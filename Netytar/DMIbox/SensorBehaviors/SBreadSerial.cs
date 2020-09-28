using NeeqDMIs.ATmega;

namespace Netytar.DMIbox.SensorBehaviors
{
    public class SBreadSerial : ISensorReaderBehavior
    {
        private string cose = "";
            
        public void ReceiveSensorRead(string val)
        {
           cose = val;
           Rack.NetytarDMIBox.TestString = cose.Replace("$", "\n");
        }

        /*
         * Gyro max values: 32767, -32768
         */
        
    }
}
