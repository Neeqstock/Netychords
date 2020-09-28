using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netytar
{
    public class KBsimulateBlow : AKeyboardBehavior
    {
        private VKeyCodes keyBlow = VKeyCodes.Space;

        private bool blowing = false;
        int returnVal = 0;

        public override int ReceiveEvent(RawInputEventArgs e)
        {
            returnVal = 0;

            if(Rack.NetytarDMIBox.NetytarControlMode == NetytarControlModes.Keyboard)
            {
                if (e.VirtualKey == (ushort)keyBlow && e.KeyPressState == KeyPressState.Down)
                {
                    blowing = true;
                    returnVal = 1;
                    Rack.NetytarDMIBox.NetytarMainWindow.BreathSensorValue = 127;
                }
                else if (e.VirtualKey == (ushort)keyBlow && e.KeyPressState == KeyPressState.Up)
                {
                    blowing = false;
                    returnVal = 1;
                    Rack.NetytarDMIBox.NetytarMainWindow.BreathSensorValue = 0;
                }
                Rack.NetytarDMIBox.Blow = blowing;
            }

            return returnVal;
        }
    }
}
