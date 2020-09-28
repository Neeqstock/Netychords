using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MIDI;
using NeeqDMIs.Music;
using Netytar.DMIbox.KeyboardBehaviors;
using Netytar.DMIbox.SensorBehaviors;
using Netytar.DMIbox.TobiiBehaviors;
using System;
using System.Windows.Interop;
using Tobii.Interaction.Framework;

namespace Netytar.DMIbox
{
    public class NetytarSetup
    {
        public NetytarSetup(MainWindow window)
        {
            Rack.NetytarDMIBox.NetytarMainWindow = window;
        }

        public void Setup()
        {
            // KEYBOARD MODULE
            IntPtr windowHandle = new WindowInteropHelper(Rack.NetytarDMIBox.NetytarMainWindow).Handle;
            Rack.NetytarDMIBox.KeyboardModule = new KeyboardModuleWPF(windowHandle);

            // MIDI
            Rack.NetytarDMIBox.MidiModule = new MidiModuleNAudio(1, 1);
            MidiDeviceFinder midiDeviceFinder = new MidiDeviceFinder(Rack.NetytarDMIBox.MidiModule);
            midiDeviceFinder.SetToLastDevice();

            // EYETRACKER
            if(Rack.NetytarDMIBox.Eyetracker == Eyetracker.Tobii)
            {
                Rack.NetytarDMIBox.TobiiModule = new TobiiModule(GazePointDataMode.Unfiltered);
                Rack.NetytarDMIBox.TobiiModule.Start();
                Rack.NetytarDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBpitchPlay(10, 15, 1.5f, 30f));
                Rack.NetytarDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBvelocityPlay(8, 12, 2f, 120f, 0.2f));
            }

            if(Rack.NetytarDMIBox.Eyetracker == Eyetracker.Eyetribe)
            {
                Rack.NetytarDMIBox.EyeTribeModule = new EyeTribeModule();
                Rack.NetytarDMIBox.EyeTribeModule.Start();
                Rack.NetytarDMIBox.EyeTribeModule.MouseEmulator = new MouseEmulator(new NoFilter());
                Rack.NetytarDMIBox.EyeTribeModule.MouseEmulatorGazeMode = GazeMode.Raw;
            }


            // MISCELLANEOUS
            Rack.NetytarDMIBox.SensorReader = new SensorModule("COM", 9600);

            // BEHAVIORS
            Rack.NetytarDMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBemulateMouse());
            Rack.NetytarDMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBsimulateBlow());
            Rack.NetytarDMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBselectScale());

            Rack.NetytarDMIBox.TobiiModule.BlinkBehaviors.Add(new EBBselectScale(Rack.NetytarDMIBox.NetytarMainWindow));
            Rack.NetytarDMIBox.TobiiModule.BlinkBehaviors.Add(new EBBrepeatNote());

            Rack.NetytarDMIBox.SensorReader.Behaviors.Add(new SBbreathSensor(20, 28, 1.5f)); // 15 20
            //Rack.DMIBox.SensorReader.Behaviors.Add(new SBaccelerometerTest());
            Rack.NetytarDMIBox.SensorReader.Behaviors.Add(new SBreadSerial());


            // SURFACE
            IDimension dimension = new DimensionInvert();
            IColorCode colorCode = new ColorCodeStandard();
            IButtonsSettings buttonsSettings = new ButtonsSettingsInvert();
            NetytarSurfaceDrawModes drawMode = NetytarSurfaceDrawModes.AllLines;

            Rack.NetytarDMIBox.AutoScroller = new AutoScroller(Rack.NetytarDMIBox.NetytarMainWindow.scrlNetytar, 0, 100, new ExpDecayingPointFilter(0.1f)); // OLD was 100, 0.1f
            Rack.NetytarDMIBox.NetytarSurface = new NetytarSurface(Rack.NetytarDMIBox.NetytarMainWindow.canvasNetytar, dimension, colorCode, buttonsSettings, drawMode);
            Rack.NetytarDMIBox.NetytarSurface.DrawButtons();
            Rack.NetytarDMIBox.NetytarSurface.Scale = ScalesFactory.Cmaj;
        }
    }
}
