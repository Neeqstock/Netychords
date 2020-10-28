using NeeqDMIs.ATmega;
using System.Globalization;
using System.Windows;

namespace HANDMIsTestSuite.Behaviors.Sensor
{
    public class HSreadSerial : ISensorReaderBehavior
    {
        private string[] split;

        public void ReceiveSensorRead(string val)
        {
            if (val.Contains("%"))
            {
                //MessageBox.Show("HeadTracker sends write request");
                Rack.DMIBox.BreathSensorModule.Write("A");
            }
            else if (val.StartsWith("$"))
            {
                //MessageBox.Show("Head tracker sends this values string:" + val);
                val = val.Replace("$", string.Empty);
                split = val.Split('!');

                Rack.DMIBox.HeadTrackerData.Yaw = -double.Parse(split[0], CultureInfo.InvariantCulture);
                Rack.DMIBox.HeadTrackerData.Pitch = double.Parse(split[1], CultureInfo.InvariantCulture);
                Rack.DMIBox.HeadTrackerData.Roll = double.Parse(split[2], CultureInfo.InvariantCulture);
            }
            Rack.DMIBox.Str_HeadTrackerRaw = Rack.DMIBox.HeadTrackerData.Yaw + "\n" + Rack.DMIBox.HeadTrackerData.Pitch + "\n" + Rack.DMIBox.HeadTrackerData.Roll + "\n";
            Rack.DMIBox.Str_HeadTrackerCalib = Rack.DMIBox.HeadTrackerData.TranspYaw + "\n" + Rack.DMIBox.HeadTrackerData.TranspPitch + "\n" + Rack.DMIBox.HeadTrackerData.TranspRoll + "\n";

        }
    }
}