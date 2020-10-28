using HANDMIsTestSuite.Modules.Polling;
using MicroLibrary;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;

namespace HANDMIsTestSuite.Modules
{
    public delegate void Success();
    public delegate void Failure();

    public class PollingModuleStopwatch
    {
        public event Success success;
        public event Failure failure;

        private TestSuiteDMIBox DMIBox;

        private MicroTimer stabilityTimer;
        private MicroTimer selectionTimer;
        private MicroTimer restTimer;

        private MicroStopwatch stopwatch;

        private List<StopWatchStamp> stampsList;

        private int stabilitySeconds = 0;
        private int selectionSeconds = 0;
        private int restSeconds = 0;

        private int totalStampsCounter = 0;

        private bool isInTarget = false;

        private int targetHalfWidth;

        public bool Started { get; private set; } = false;
        public PollingModuleStopwatch(Success successEvent, Failure failureEvent)
        {
            this.success += successEvent;
            DMIBox = Rack.DMIBox;
            targetHalfWidth = GlobalValues.TargetWidth / 2;
            stopwatch = new MicroStopwatch();

            stabilityTimer = new MicroTimer();
            stabilityTimer.Interval = 1000000;
            stabilityTimer.MicroTimerElapsed += StabilityTimer_MicroTimerElapsed;

            selectionTimer = new MicroTimer();
            selectionTimer.Interval = 1000000;
            selectionTimer.MicroTimerElapsed += SelectionTimer_MicroTimerElapsed;

            restTimer = new MicroTimer();
            restTimer.Interval = 1000000;
            restTimer.MicroTimerElapsed += RestTimer_MicroTimerElapsed;

            stampsList = new List<StopWatchStamp>();
        }
        public void StartFitts()
        {
            stampsList.Clear();
            totalStampsCounter = 0;

            stabilitySeconds = 0;
            selectionSeconds = 0;

            stopwatch.Reset();
            stopwatch.Start();

            selectionTimer.Start();

            isInTarget = false;
            DMIBox.Locked = LockedStates.Not;

            Started = true;

            Rack.DMIBox.TestState = TestStates.Fitts;
        }
        public void StartRestChallenge()
        {
            restSeconds = 0;

            DMIBox.Locked = LockedStates.Not;

            Started = true;

            Rack.DMIBox.TestState = TestStates.RestChallenge;
        }
        public void StopFitts()
        {
            totalStampsCounter = 0;

            stabilitySeconds = 0;
            selectionSeconds = 0;

            stopwatch.Stop();
            stabilityTimer.Stop();
            selectionTimer.Stop();

            isInTarget = false;
            DMIBox.Locked = LockedStates.Not;

            Started = false;
        }
        public void StopRestChallenge()
        {
            totalStampsCounter = 0;

            restSeconds = 0;

            restTimer.Stop();

            isInTarget = false;
            DMIBox.Locked = LockedStates.Not;

            Started = false;
        }
        public void ReceiveStopwatchStamp()
        {
            if (Rack.DMIBox.TestStarted && Rack.DMIBox.TestState == TestStates.Fitts && stabilitySeconds < GlobalValues.StabilityTimeTotal)
            {
                totalStampsCounter++;
                stampsList.Add(new StopWatchStamp(totalStampsCounter, DMIBox.Locked, Rack.DMIBox.XTargetLeft - (int)Rack.DMIBox.XBarLeft + targetHalfWidth, DMIBox.XCursorCenter - (int)Rack.DMIBox.XBarLeft, stopwatch.ElapsedMicroseconds));

                if (DMIBox.Locked != LockedStates.Selected)
                {
                    if (IsLockConditionSatisfied())
                    {
                        DMIBox.Locked = LockedStates.Locked;
                        stabilityTimer.Start();
                    }
                    else
                    {
                        DMIBox.Locked = LockedStates.Not;
                        stabilitySeconds = 0;
                        stabilityTimer.Stop();
                        stabilityTimer.Abort();
                    }
                }
            }
            else if (Rack.DMIBox.TestStarted && Rack.DMIBox.TestState == TestStates.RestChallenge)
            {
                if (IsLockConditionSatisfied())
                {
                    DMIBox.Locked = LockedStates.Locked;
                    restTimer.Start();
                }
                else
                {
                    DMIBox.Locked = LockedStates.Not;
                    restSeconds = 0;
                    restTimer.Stop();
                    restTimer.Abort(); // LAST
                }
            }
        }
        private void SelectionTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs timerEventArgs)
        {
            selectionSeconds++;
            if (selectionSeconds >= GlobalValues.SelectionTimerFail)
            {
                ProcessFittsFailure();
            }
        }
        private void StabilityTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs timerEventArgs)
        {
            stabilitySeconds++;

            if (stabilitySeconds >= GlobalValues.SelectionTimeRequired) //(DMIBox.Locked != LockedStates.Selected && stabilitySeconds >= GlobalValues.SelectionTimeRequired)
            {
                selectionTimer.Stop();
                restTimer.Abort();
                selectionSeconds = 0;

                DMIBox.Locked = LockedStates.Selected;
            }

            if (stabilitySeconds >= GlobalValues.StabilityTimeTotal) //(DMIBox.Locked == LockedStates.Selected && stabilitySeconds >= GlobalValues.StabilityTimeTotal)
            {
                Rack.DMIBox.TestStarted = false;
                ProcessFittsSuccess();
            }

        }
        private void RestTimer_MicroTimerElapsed(object sender, MicroTimerEventArgs timerEventArgs)
        {
            restSeconds++;

            if (DMIBox.Locked == LockedStates.Locked && restSeconds >= GlobalValues.RestTime)
            {
                ProcessRestEnd();
            }
        }
        private void ProcessFittsSuccess()
        {
            Rack.DMIBox.TestStarted = false;
            Rack.DMIBox.TestState = TestStates.Pause;
            StopFitts();

            if (Rack.DMIBox.TrialNumber > GlobalValues.TrainingTrialsNumber)
            {
                Rack.DMIBox.TestState = TestStates.Printing;
                Rack.DMIBox.PrinterModule.PrintList(stampsList, DMIBox.SubjectName, DMIBox.ControlMode.ToString(), DMIBox.TrialNumber);
            }

            DMIBox.Locked = LockedStates.Not;
            Rack.DMIBox.TestState = TestStates.RestWaitInput;

            stampsList.Clear();

            success?.Invoke();
            Rack.DMIBox.TestStarted = true;
            GlobalValues.SoundTrialEnd.Play();
        }
        private void ProcessFittsFailure()
        {
            Rack.DMIBox.TestStarted = false;
            Rack.DMIBox.TestState = TestStates.Pause;
            StopFitts();

            if (Rack.DMIBox.TrialNumber > GlobalValues.TrainingTrialsNumber)
            {
                Rack.DMIBox.TestState = TestStates.Printing;
                Rack.DMIBox.PrinterModule.PrintFailure(stampsList, DMIBox.SubjectName, DMIBox.ControlMode.ToString(), DMIBox.TrialNumber);

            }

            DMIBox.Locked = LockedStates.Not;
            Rack.DMIBox.TestState = TestStates.RestWaitInput;

            stampsList.Clear();

            success?.Invoke();
            Rack.DMIBox.TestStarted = true;
            GlobalValues.SoundTrialFailure.Play();
        }
        private void ProcessRestEnd()
        {
            Rack.DMIBox.TestStarted = false;
            Rack.DMIBox.TestState = TestStates.Pause;

            StopRestChallenge();

            DMIBox.Locked = LockedStates.Not;
            Rack.DMIBox.TestState = TestStates.WaitForFitts;

            success?.Invoke();
        }
        private bool IsLockConditionSatisfied()
        {
            return DMIBox.XCursorCenter > DMIBox.XTargetLeft && DMIBox.XCursorCenter < DMIBox.XTargetRight;
        }
    }
}
