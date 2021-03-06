﻿using NeeqDMIs.ATmega;
using System.Globalization;

namespace Netychords.Behaviors.Sensor
{
    public class HSreadSerial : ISensorReaderBehavior
    {
        private string[] split;

        public void ReceiveSensorRead(string val)
        {
            if (val.Contains("%")) //Arduino need some signal to start the serial flow
            {
                Rack.NetychordsDMIBox.HeadTrackerModule.Write("A");
            }
            else if (val.StartsWith("$")) //Input data is formatted as $yaw!pitch!roll ($0.00!0.00!0.00)
            {
                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                //Extracting the single data from the input string
                Rack.NetychordsDMIBox.HTData.Yaw = double.Parse(split[0], CultureInfo.InvariantCulture);
                Rack.NetychordsDMIBox.HTData.Pitch = double.Parse(split[1], CultureInfo.InvariantCulture);
                Rack.NetychordsDMIBox.HTData.Roll = double.Parse(split[2], CultureInfo.InvariantCulture);

                //Strumming is elaborated only while the head position is centered along the pitch axis
                //if (Rack.NetychordsDMIBox.HeadTrackerData.Pitch <= Rack.NetychordsDMIBox.MainWindow.centerPitchZone.Value && Rack.NetychordsDMIBox.HeadTrackerData.Pitch >= -Rack.NetychordsDMIBox.MainWindow.centerPitchZone.Value)
                //{
                    Rack.NetychordsDMIBox.ElaborateStrumming();
                //}
            }

            //Debugging variables
            Rack.NetychordsDMIBox.Str_HeadTrackerRaw = Rack.NetychordsDMIBox.HTData.Yaw.ToString();
            Rack.NetychordsDMIBox.Str_HeadTrackerCalib = Rack.NetychordsDMIBox.HTData.TranspYaw.ToString();
        }
    }
}