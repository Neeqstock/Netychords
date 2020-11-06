using NeeqDMIs.ATmega;
using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MIDI;
using Netytar.Behaviors.Sensor;
using Netytar.DMIBox.KeyboardBehaviors;
using System;
using System.Windows.Interop;
using Tobii.Interaction.Framework;

namespace Netytar.DMIBox
{
    public class CalibrateSetup
    {
        public CalibrateSetup(MainWindow window)
        {
            Rack.NetychordsDMIBox.MainWindow = window;
        }

        public void Setup()
        {

            // MODULES
            //IntPtr windowHandle = new WindowInteropHelper(Rack.NetychordsDMIBox.TestMainWindow).Handle;
            Rack.NetychordsDMIBox.HeadTrackerModule = new SensorModule("COM", 115200);

            // BEHAVIORS 
            Rack.NetychordsDMIBox.HeadTrackerModule.Behaviors.Add(new HSreadSerial());

            //SURFACE
            //Rack.NetychordsDMIBox.AutoScroller = new AutoScroller(Rack.NetychordsDMIBox.MainWindow.scrlNetychords, 0, 100, new ExpDecayingPointFilter(0.1f));
            IDimension dimension = new DimensionCalibration();
            IColorCode colorCode = new ColorCodeStandard();
            IButtonsSettings buttonsSettings = new ButtonsSettingsCalibrate();

            Rack.NetychordsDMIBox.CalibrationSurface = new CalibrationSurface(Rack.NetychordsDMIBox.MainWindow.canvasNetychords, dimension, colorCode, buttonsSettings);

        }
    }
}
