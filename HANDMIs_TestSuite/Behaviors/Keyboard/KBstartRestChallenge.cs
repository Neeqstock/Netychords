using HANDMIsTestSuite;
using NeeqDMIs.Keyboard;
using RawInput_dll;

namespace HANDMIs_TestSuite.Behaviors.Keyboard
{
    class KBstartRestChallenge : AKeyboardBehavior
    {
        private string key = LVKeyNames.LCONTROL;

        public override int ReceiveEvent(RawInputEventArg e)
        {
            if (e.KeyPressEvent.VKeyName == key && e.KeyPressEvent.KeyPressState == LKeyPressStates.MAKE)
            {
                if(Rack.DMIBox.TestState == TestStates.RestWaitInput)
                {
                    Rack.DMIBox.TestState = TestStates.WaitForRestChallenge;
                }
            }
            return 1;
        }
    }
}
