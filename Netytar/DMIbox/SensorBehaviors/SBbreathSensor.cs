using NeeqDMIs.ATmega;
using System.Globalization;

namespace Netytar.DMIbox.SensorBehaviors
{
    public class SBbreathSensor : ISensorReaderBehavior
    {
        private int v = 1;
        private int offThresh;
        private int onThresh;
        private float sensitivity;

        public SBbreathSensor(int offThresh, int onThresh, float sensitivity)
        {
            this.offThresh = offThresh;
            this.onThresh = onThresh;
            this.sensitivity = sensitivity;
        }

        public void ReceiveSensorRead(string val)
        {
            if(Rack.NetytarDMIBox.NetytarControlMode == NetytarControlModes.BreathSensor)
            {
                float b = 0;

                try
                {
                    b = float.Parse(val, CultureInfo.InvariantCulture.NumberFormat);
                }
                catch
                {

                }

                v = (int)(b / 3);

                Rack.NetytarDMIBox.NetytarMainWindow.BreathSensorValue = v;
                Rack.NetytarDMIBox.Pressure = (int)(v * 2 * sensitivity);
                Rack.NetytarDMIBox.Modulation = (int)(v / 8 * sensitivity);

                if (v > onThresh && Rack.NetytarDMIBox.Blow == false)
                {
                    Rack.NetytarDMIBox.Blow = true;
                    //NetytarRack.DMIBox.Pressure = 110;
                }

                if (v < offThresh)
                {
                    Rack.NetytarDMIBox.Blow = false;
                }
            }
            
        }
    }
}
