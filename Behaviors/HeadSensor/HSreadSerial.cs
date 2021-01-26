using NeeqDMIs.ATmega;
using System;
using System.Globalization;

namespace Netytar.Behaviors.Sensor
{
    public class HSreadSerial : ISensorReaderBehavior
    {
        private string[] split;
        private double lastYaw;

        public void ReceiveSensorRead(string val)
        {
            if (val.Contains("%"))
            {
                //MessageBox.Show("HeadTracker sends write request");
                Rack.NetychordsDMIBox.HeadTrackerModule.Write("A");
            }
            else if (val.StartsWith("$"))
            {
                //MessageBox.Show("Head tracker sends this values string:" + val);
                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                Rack.NetychordsDMIBox.HeadTrackerData.Yaw = double.Parse(split[0], CultureInfo.InvariantCulture);
                /*if (Rack.NetychordsDMIBox.calibrateStarted && !Rack.NetychordsDMIBox.calibrateEnded)
                {
                    Rack.NetychordsDMIBox.HeadTrackerData.CalibrationYaw = double.Parse(split[0], CultureInfo.InvariantCulture);
                }*/
                Rack.NetychordsDMIBox.HeadTrackerData.Pitch = double.Parse(split[1], CultureInfo.InvariantCulture);
                Rack.NetychordsDMIBox.HeadTrackerData.Roll = double.Parse(split[2], CultureInfo.InvariantCulture);

                if (Rack.NetychordsDMIBox.HeadTrackerData.Pitch <= Rack.NetychordsDMIBox.MainWindow.centerPitchZone.Value && Rack.NetychordsDMIBox.HeadTrackerData.Pitch >= - Rack.NetychordsDMIBox.MainWindow.centerPitchZone.Value)
                {
                  Rack.NetychordsDMIBox.elaborateStrumming();
                }
            }
            
            Rack.NetychordsDMIBox.Str_HeadTrackerRaw = Rack.NetychordsDMIBox.HeadTrackerData.Yaw + "";// + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.Pitch + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.Roll + "\n";
            Rack.NetychordsDMIBox.Str_HeadTrackerCalib = Rack.NetychordsDMIBox.HeadTrackerData.TranspYaw + "";// "\n" + Rack.NetychordsDMIBox.HeadTrackerData.TranspPitch + "\n" + Rack.NetychordsDMIBox.HeadTrackerData.TranspRoll + "\n";
        }

    }
}