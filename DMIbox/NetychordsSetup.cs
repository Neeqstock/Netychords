using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Eyetracking;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MIDI;
using Netychords.Behaviors.Sensor;
using Netychords.DMIBox.KeyboardBehaviors;
using System;
using System.Windows.Interop;
using Tobii.Interaction.Framework;
using NeeqDMIs.Eyetracking.PointFilters;
using Netychords.Behaviors.Eyetracker;
using Eyerpheus.Controllers.Graphics;
using Netychords.Surface;

namespace Netychords.DMIBox
{
    public class NetychordsSetup
    {
        public NetychordsSetup(MainWindow window)
        {
            Rack.NetychordsDMIBox.MainWindow = window;
        }

        public void Setup()
        {
            // KEYBOARD MODULE
            IntPtr windowHandle = new WindowInteropHelper(Rack.NetychordsDMIBox.MainWindow).Handle;
            Rack.NetychordsDMIBox.KeyboardModule = new KeyboardModule(windowHandle);

            // MIDI
            Rack.NetychordsDMIBox.MidiModule = new MidiModuleNAudio(1, 1);
            MidiDeviceFinder midiDeviceFinder = new MidiDeviceFinder(Rack.NetychordsDMIBox.MidiModule);
            midiDeviceFinder.SetToLastDevice();

            // EYETRACKER
            if(Rack.NetychordsDMIBox.Eyetracker == EyetrackerModels.Tobii)
            {
                Rack.NetychordsDMIBox.TobiiModule = new TobiiModule(GazePointDataMode.Unfiltered);
                Rack.NetychordsDMIBox.TobiiModule.Start();
                //Rack.NetychordsDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBpitchPlay(10, 15, 1.5f, 30f));
                //Rack.NetychordsDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBvelocityPlay(8, 12, 2f, 120f, 0.2f));
            }

            if(Rack.NetychordsDMIBox.Eyetracker == EyetrackerModels.EyeTribe)
            {
                Rack.NetychordsDMIBox.EyeTribeModule = new EyeTribeModule();
                Rack.NetychordsDMIBox.EyeTribeModule.Start();
                Rack.NetychordsDMIBox.EyeTribeModule.MouseEmulator = new MouseEmulator(new PointFilterBypass());
                Rack.NetychordsDMIBox.EyeTribeModule.MouseEmulatorGazeMode = GazeMode.Raw;
            }

            Rack.NetychordsDMIBox.TobiiModule.BlinkBehaviors.Add(new BBDoubleCloseStopChords());

            // MODULES
            //IntPtr windowHandle = new WindowInteropHelper(Rack.NetychordsDMIBox.TestMainWindow).Handle;
            Rack.NetychordsDMIBox.HeadTrackerModule = new SensorModule("COM", 115200);

            // BEHAVIORS
            Rack.NetychordsDMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBplayStop());
            Rack.NetychordsDMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBemulateMouse());
            Rack.NetychordsDMIBox.HeadTrackerModule.Behaviors.Add(new HSreadSerial());

            //SURFACE
            Rack.NetychordsDMIBox.AutoScroller = new AutoScroller(Rack.NetychordsDMIBox.MainWindow.scrlNetychords, 0, 100, new PointFilterMAExpDecaying(0.1f));
            IDimension dimension = new DimensionInvert();
            IColorCode colorCode = new ColorCodeStandard();
            IButtonsSettings buttonsSettings = new ButtonsSettingsChords();

            NetychordsSurfaceDrawModes drawMode = NetychordsSurfaceDrawModes.NoLines;

            Rack.NetychordsDMIBox.NetychordsSurface = new NetychordsSurface(Rack.NetychordsDMIBox.MainWindow.canvasNetychords, dimension, colorCode, buttonsSettings, drawMode);
            Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();

            Rack.NetychordsDMIBox.NetychordsSurface.HtFeedbackModule = new HTFeedbackModule(Rack.NetychordsDMIBox.NetychordsSurface.Canvas, HTFeedbackModule.HTFeedbackModes.DeadZone);
        }
    }
}
