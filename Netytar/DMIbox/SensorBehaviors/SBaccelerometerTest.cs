using NeeqDMIs.ATmega;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Netytar.DMIbox.SensorBehaviors
{
    public class SBaccelerometerTest : ISensorReaderBehavior
    {
        string[] para = new string[3];

        public SBaccelerometerTest()
        {

        }

        public void ReceiveSensorRead(string val)
        {
            para = val.Split('/');

            if(para.Length == 6)
            {
                Rack.NetytarDMIBox.GyroX = int.Parse(para[0]);
                Rack.NetytarDMIBox.GyroY = int.Parse(para[1]);
                Rack.NetytarDMIBox.GyroZ = int.Parse(para[2]);
                Rack.NetytarDMIBox.AccX = int.Parse(para[3]);
                Rack.NetytarDMIBox.AccY = int.Parse(para[4]);
                Rack.NetytarDMIBox.AccZ = int.Parse(para[5]);

                PrintIndicators();

                Rack.NetytarDMIBox.MidiModule.SetPitchBend((Rack.NetytarDMIBox.GyroCalibX / 2 + 8192));
            }
            else
            {
                // missing values
            }
            
        }

        private void PrintIndicators()
        {
            Rack.NetytarDMIBox.TestString = "X: " + Rack.NetytarDMIBox.AccCalibX + "\nY: " + Rack.NetytarDMIBox.AccCalibY + "\nZ: " + Rack.NetytarDMIBox.AccCalibZ;
        }

        private int ReadValue(string val)
        {
            return int.Parse(val, CultureInfo.InvariantCulture.NumberFormat);
        }

        /*
         * Gyro max values: 32767, -32768
         */
        
    }
}
