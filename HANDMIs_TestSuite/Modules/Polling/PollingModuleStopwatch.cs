using HANDMIsTestSuite.Modules.Polling;
using MicroLibrary;
using System.Collections.Generic;

namespace HANDMIsTestSuite.Modules
{
    public class PollingModuleStopwatchOld
    {
        public event Success success;

        private TestSuiteDMIBox DMIBox;

        private MicroTimer successTimer;
        private int successCyclesCounter = 0;
        private int successStampsCounter = 0;
        private long stampsCounter = 0;
        private List<StopWatchStamp> stampsList;
        private int targetHalfDim;
        private MicroStopwatch stopwatch;

        private bool started;

        public bool Started { get => started; }

        public PollingModuleStopwatchOld(Success successEvent)
        {
            this.success += successEvent;
            DMIBox = Rack.DMIBox;
            targetHalfDim = GlobalValues.TargetWidth / 2;
            stopwatch = new MicroStopwatch();

            successTimer = new MicroTimer();
            successTimer.Interval = 1000;
            successTimer.MicroTimerElapsed += SuccessTimer_MicroTimerElapsed;

            stampsList = new List<StopWatchStamp>();
        }
        private bool LockConditionSatisfied()
        {
            switch (DMIBox.Direction)
            {
                case ControlDirection.X:
                    return DMIBox.XCursorCenter > DMIBox.XTargetLeft && DMIBox.XCursorCenter < DMIBox.XTargetRight;
                case ControlDirection.Y:
                    //return DMIBox.YCursorCenter > DMIBox.YTargetTop && DMIBox.YCursorCenter < DMIBox.YTargetBottom;
                default:
                    return false;
            }
        }

        public void StoreStopwatchStamp()
        {
            if (Rack.DMIBox.TestStarted)
            {
                stampsCounter++;

                switch (DMIBox.Direction)
                {
                    case ControlDirection.X:
                        stampsList.Add(new StopWatchStamp(stampsCounter, successStampsCounter, DMIBox.Locked, Rack.DMIBox.XTargetLeft - (int)Rack.DMIBox.XBarLeft + targetHalfDim, DMIBox.XCursorCenter - (int)Rack.DMIBox.XBarLeft, stopwatch.ElapsedMicroseconds, false));
                        break;
                    case ControlDirection.Y:
                        //stampsList.Add(new StopWatchStamp(stampsCounter, successStampsCounter, DMIBox.Locked, Rack.DMIBox.YTargetTop - (int)Rack.DMIBox.YBarTop + targetHalfDim, DMIBox.YCursorCenter - (int)Rack.DMIBox.YBarTop, stopwatch.ElapsedMicroseconds));
                        break;
                }

                if (LockConditionSatisfied())
                {
                    DMIBox.Locked = true;
                    successStampsCounter++;
                    successTimer.Start();
                }
                else
                {
                    successTimer.Stop();
                    successCyclesCounter = 0;
                    DMIBox.Locked = false;
                    successStampsCounter = 0;
                }
            }
            
        }

        public void StartPoll()
        {
            successStampsCounter = 0;
            stampsCounter = 0;
            successCyclesCounter = 0;
            stopwatch.Reset();
            stopwatch.Start();
            started = true;
        }

        public void StopPoll()
        {
            started = false;
            successTimer.Stop();
            stopwatch.Stop();
            stampsCounter = 0;
            successStampsCounter = 0;

    }

        private void SuccessTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs timerEventArgs)
        {
            successCyclesCounter++;
            if(successCyclesCounter >= GlobalValues.SelectionTimeRequired)
            {
                ProcessSuccess();
            }

        }

        private void ProcessSuccess()
        {
            Rack.DMIBox.TestStarted = false;
            StopPoll();

            if (Rack.DMIBox.TrialNumber > GlobalValues.TrainingTrialsNumber)
            {
                Rack.DMIBox.PrinterModule.PrintList(stampsList, DMIBox.SubjectName, DMIBox.ControlMode.ToString(), DMIBox.TrialNumber);
            }

            DMIBox.Locked = false;
            DMIBox.Success = true;
            successCyclesCounter = 0;

            stampsList.Clear();

            success?.Invoke();
        }
    }
}
