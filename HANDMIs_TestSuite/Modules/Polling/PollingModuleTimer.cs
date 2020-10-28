using HANDMIsTestSuite.Modules.Polling;
using MicroLibrary;
using System.Collections.Generic;

namespace HANDMIsTestSuite.Modules
{
    public delegate void Success();

    public class PollingModuleTimer
    {
        public event Success success;

        private TestSuiteDMIBox DMIBox;

        private MicroTimer timer;
        private int successCounter = 0;
        private long pollCounter = 0;
        private List<TimerStamp> stampsList;
        private int targetHalfDim;

        public PollingModuleTimer(Success successEvent)
        {
            this.success += successEvent;
            DMIBox = Rack.DMIBox;
            targetHalfDim = GlobalValues.TargetWidth / 2;
        }

        public void StopPoll()
        {
            timer.Stop();
            successCounter = 0;
            pollCounter = 0;
        }

        private void StorePoll(long lateBy)
        {
            switch (DMIBox.Direction)
            {
                case ControlDirection.X:
                    stampsList.Add(new TimerStamp(pollCounter, successCounter, DMIBox.Locked, Rack.DMIBox.XTargetLeft + targetHalfDim - (int)Rack.DMIBox.XBarLeft, DMIBox.XCursorCenter - (int)Rack.DMIBox.XBarLeft, lateBy));
                    break;
                case ControlDirection.Y:
                    //stampsList.Add(new TimerStamp(pollCounter, successCounter, DMIBox.Locked, Rack.DMIBox.YTargetTop + targetHalfDim - (int)Rack.DMIBox.YBarTop, DMIBox.YCursorCenter - (int)Rack.DMIBox.YBarTop, lateBy));
                    break;
            }
        }

        public void StartPoll()
        {
            stampsList = new List<TimerStamp>();

            timer = new MicroTimer();
            timer.Interval = 1000;
            timer.MicroTimerElapsed += Timer_MicroTimerElapsed;
            timer.Start();
        }

        private void Timer_MicroTimerElapsed(object sender, MicroTimerEventArgs timerEventArgs)
        {
            pollCounter++;
            if (LockConditionSatisfied())
            {
                DMIBox.Locked = true;
                successCounter++;
            }
            else
            {
                DMIBox.Locked = false;
                successCounter = 0;
            }

            StorePoll(timerEventArgs.TimerLateBy);


            if (successCounter >= GlobalValues.SelectionTimeRequired)
            {
                ProcessSuccess();
            }
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

        private void ProcessSuccess()
        {
            Rack.DMIBox.TestStarted = false;
            timer.Stop();

            if (Rack.DMIBox.TrialNumber > GlobalValues.TrainingTrialsNumber)
            {
                Rack.DMIBox.PrinterModule.PrintList(stampsList, DMIBox.SubjectName, DMIBox.ControlMode.ToString(), DMIBox.TrialNumber);
            }
            stampsList.Clear();

            DMIBox.Locked = false;
            DMIBox.Success = true;
            success?.Invoke();

        }
    }
}
