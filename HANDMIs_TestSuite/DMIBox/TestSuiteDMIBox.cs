using EyeTribe.ClientSdk.Data;
using HANDMIs_TestSuite.Utils;
using HANDMIsTestSuite.Modules;
using HANDMIsTestSuite.Modules.GuideModule;
using HANDMIsTestSuite.Modules.Polling;
using HANDMIsTestSuite.MouseSpace;
using HANDMIsTestSuite.Printer;
using NeeqDMIs.ATmega;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HANDMIsTestSuite
{
    class TestSuiteDMIBox : NeeqDMIs.DMIBox
    {
        public TestSuiteDMIBox(MainWindow mainWindow)
        {
            this.TestMainWindow = mainWindow;
            this.XCanvas = mainWindow.XObjectsCanvas;
        }

        #region Extra sensors
        public SensorModule BreathSensorModule { get; set; }
        public SensorModule HeadTrackerModule { get; set; }
        #endregion

        #region Shared values

        public void Dispose()
        {
            try
            {
                TobiiModule.Dispose();
            }
            catch
            {

            }
            try
            {
                BreathSensorModule.Disconnect();
            }
            catch
            {

            }

        }
        #endregion

        public long BreathSensorMax { get; set; } = 500;
        public string Str_HeadTrackerRaw { get; set; } = "Test";
        public string Str_HeadTrackerCalib { get; set; } = "Test";
        public MainWindow TestMainWindow { get; set; }
        public Canvas XCanvas { get; set; }
        public MouseModule MouseModule { get; set; }
        public HeadTrackerData HeadTrackerData { get; set; } = new HeadTrackerData();
        private ControlModes controlMode;
        public ControlModes ControlMode
        {
            get { return controlMode; }
            set
            {
                Direction = value.GetDirection();
                controlMode = value;
                if(value == ControlModes.MouseX)
                {
                    MouseModule?.Start();
                }
                else
                {
                    MouseModule?.Stop();
                }
            }
        }
        public long BreathSensorValue { get; set; } = 0;
        public long BreathSensorZero { get; set; } = 0;
        public long BreathSensorCalib { get { return BreathSensorValue - BreathSensorZero; } }
        public ControlDirection Direction { get; private set; }
        public GuideModule GuideModule { get; set; }
        public int XCursorCenter { get; set; }
        public int XCursorLeft { get { return XCursorCenter - GlobalValues.HalfCursorWidth; } }
        public int XTargetLeft { get; set; }
        public int XTargetRight { get { return XTargetLeft + GlobalValues.TargetWidth; } set { XTargetLeft = value - GlobalValues.TargetWidth; } }
        public LockedStates Locked { get; set; } = LockedStates.Not;
        public int TrialNumber { get; set; }
        public string SubjectName { get; set; } = "NoName";
        public PrinterModule PrinterModule { get; set; }
        public PollingMode PollingMode { get; set; } = PollingMode.Timer;
        public long XCursorValue
        {
            set
            {
                //if(value < 0)
                //{
                 //   XCursorCenter = (int)(XBarLeft);
                //}
                //else if(value > GlobalValues.BarWidth)
                //{
                //    XCursorCenter = (int)(XBarLeft + GlobalValues.BarWidth);
                //}
                //else
               //{
                  XCursorCenter = (int)(XBarLeft + value);
                //}
                
            }
        }
        public double XBarLeft { get; set; }
        public double XBarRight { get; set; }
        //public PollingModuleTimer PollingModuleTimer { get; set; }
        public PollingModuleStopwatch PollingModuleStopwatch { get; set; }
        private int breathSensorPortNumber = 0;
        public int BreathSensorPortNumber
        {
            get
            {
                return breathSensorPortNumber;
            }
            set
            {
                if (value < 0)
                {
                    breathSensorPortNumber = 0;
                }
                else
                {
                    breathSensorPortNumber = value;
                }
            }
        }
        private int headTrackerPortNumber = 0;
        public int HeadTrackerPortNumber
        {
            get
            {
                return headTrackerPortNumber;
            }
            set
            {
                if(value < 0)
                {
                    headTrackerPortNumber = 0;
                }
                else
                {
                    headTrackerPortNumber = value;
                }
            }
        }
        public bool TestStarted { get; set; } = false;
        public double ScreenWidth { get; set; } = 1920;
        public double ScreenHeight { get; set; } = 1080;
        public GazeData EyeTribeGPData { get; set; }
        public TestStates TestState { get; set; } = TestStates.Stop;
        public int DistancesIndex { get; set; } = 0;
        public int PitchInverter { get; set; } = 1;
        public int RollInverter { get; set; } = 1;
        public bool InvertPitchRoll { get; set; } = false;

        public List<TargetDistance> TargetDistances = new List<TargetDistance>()
        {
            new TargetDistance(300),
            new TargetDistance(300),
            new TargetDistance(500),
            new TargetDistance(500),
            new TargetDistance(700),
            new TargetDistance(700),
            new TargetDistance(900),
            new TargetDistance(900),
        };
    }

    public enum TestStates
    {
        Fitts,
        RestWaitInput,
        RestChallenge,
        Printing,
        Stop,
        Pause,
        WaitForFitts,
        WaitForRestChallenge,
        End
    }

    public enum LockedStates
    {
        Not,
        Locked,
        Selected
    }
}
