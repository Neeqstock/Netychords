using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MIDI;
using Netytar.DMIbox.KeyboardBehaviors;
using System;
using System.Windows.Interop;
using Tobii.Interaction.Framework;

namespace Netytar.DMIbox
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
            Rack.NetychordsDMIBox.KeyboardModule = new KeyboardModuleWPF(windowHandle);

            // MIDI
            Rack.NetychordsDMIBox.MidiModule = new MidiModuleNAudio(1, 1);
            MidiDeviceFinder midiDeviceFinder = new MidiDeviceFinder(Rack.NetychordsDMIBox.MidiModule);
            midiDeviceFinder.SetToLastDevice();

            // EYETRACKER
            if(Rack.NetychordsDMIBox.Eyetracker == Eyetracker.Tobii)
            {
                Rack.NetychordsDMIBox.TobiiModule = new TobiiModule(GazePointDataMode.Unfiltered);
                Rack.NetychordsDMIBox.TobiiModule.Start();
                //Rack.NetychordsDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBpitchPlay(10, 15, 1.5f, 30f));
                //Rack.NetychordsDMIBox.TobiiModule.HeadPoseBehaviors.Add(new HPBvelocityPlay(8, 12, 2f, 120f, 0.2f));
            }

            if(Rack.NetychordsDMIBox.Eyetracker == Eyetracker.Eyetribe)
            {
                Rack.NetychordsDMIBox.EyeTribeModule = new EyeTribeModule();
                Rack.NetychordsDMIBox.EyeTribeModule.Start();
                Rack.NetychordsDMIBox.EyeTribeModule.MouseEmulator = new MouseEmulator(new NoFilter());
                Rack.NetychordsDMIBox.EyeTribeModule.MouseEmulatorGazeMode = GazeMode.Raw;
            }


            // BEHAVIORS
            Rack.NetychordsDMIBox.KeyboardModule.KeyboardBehaviors.Add(new KBplayStop());

        }
    }
}
