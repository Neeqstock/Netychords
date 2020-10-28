using NeeqDMIs.Keyboard;
using RawInput_dll;

namespace HANDMIsTestSuite.Behaviors.Keyboard
{
    public class KBtestTimeStamp : AKeyboardBehavior
    {
        private string key = LVKeyNames.S;

        public override int ReceiveEvent(RawInputEventArg e)
        {
            if (e.KeyPressEvent.VKeyName == key && e.KeyPressEvent.KeyPressState == LKeyPressStates.MAKE)
            {
                Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
                
                return 0;
                
            }
            return 1;
        }
    }
}
