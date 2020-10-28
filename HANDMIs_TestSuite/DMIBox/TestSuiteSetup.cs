using HANDMIs_TestSuite.Behaviors.Keyboard;
using HANDMIsTestSuite.Behaviors.BreathSensor;
using HANDMIsTestSuite.Behaviors.EyeTribe;
using HANDMIsTestSuite.Behaviors.HeadSensor;
using HANDMIsTestSuite.Behaviors.Keyboard;
using HANDMIsTestSuite.Behaviors.Mouse;
using HANDMIsTestSuite.Behaviors.Sensor;
using HANDMIsTestSuite.Modules.Polling;
using HANDMIsTestSuite.MouseSpace;
using HANDMIsTestSuite.Printer;
using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Keyboard;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Interop;

namespace HANDMIsTestSuite
{
    class TestSuiteSetup
    {
        public TestSuiteSetup(MainWindow window)
        {
            Rack.DMIBox = new TestSuiteDMIBox(window);
        }

        public void Setup()
        {
            // INIT
            Rack.DMIBox.ControlMode = ControlModes.None;
            Rack.DMIBox.PollingMode = PollingMode.Stopwatch;

            // MODULES
            IntPtr windowHandle = new WindowInteropHelper(Rack.DMIBox.TestMainWindow).Handle;

            Rack.DMIBox.KeyboardModule = new KeyboardModule(windowHandle);
            Rack.DMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBtestTimeStamp());
            Rack.DMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBstartRestChallenge());

            //TestSuiteRack.DMIBox.MidiModule = new MidiModuleNAudio(1, 1);
            //MidiDeviceFinder midiDeviceFinder = new MidiDeviceFinder(TestSuiteRack.DMIBox.MidiModule);
            //midiDeviceFinder.SetToLastDevice();

            // Rack.DMIBox.TobiiModule = new TobiiModule(GazePointDataMode.Unfiltered);
            // Rack.DMIBox.TobiiModule.Start();

            Rack.DMIBox.EyeTribeModule = new EyeTribeModule();
            Rack.DMIBox.EyeTribeModule.Start();

            Rack.DMIBox.HeadTrackerModule = new SensorModule("COM", 115200);
            Rack.DMIBox.BreathSensorModule = new SensorModule("COM", 9600);

            Rack.DMIBox.MouseModule = new MouseModule(1);
            Rack.DMIBox.MouseModule.Start();

            List<TextBlock> guideBlocks = new List<TextBlock>()
            {
                Rack.DMIBox.TestMainWindow.txtXGuide
            };
            Rack.DMIBox.GuideModule = new Modules.GuideModule.GuideModule(guideBlocks, 10000, 10000);

            Rack.DMIBox.PrinterModule = new PrinterModule();

            // BEHAVIORS
            // Rack.DMIBox.TobiiModule.GazePointBehaviors.Add(new GPBgazePointX());
            // Rack.DMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBheadYaw(0.5f));

            Rack.DMIBox.EyeTribeModule.GazePointBehaviors.Add(new EyeTribeGPBgaze(GazeMode.Raw));

            Rack.DMIBox.MouseModule.MouseBehaviors.Add(new BMouseX());
            Rack.DMIBox.MouseModule.MouseBehaviors.Add(new BMouseY());
            // Rack.DMIBox.TobiiModule.MouseEmulator.Filter = new NoFilter();

            Rack.DMIBox.BreathSensorModule.Behaviors.Add(new BSbreathSensor());
            Rack.DMIBox.HeadTrackerModule.Behaviors.Add(new HSmoveCursor());


        }
    }
}
