using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using RawInputProcessor;

namespace Netytar.DMIBox.KeyboardBehaviors
{
    class KBplayStop : AKeyboardBehavior
    {
        private const VKeyCodes space = VKeyCodes.Space;
        private bool isDown = false;

        public override int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)space && e.KeyPressState == KeyPressState.Down && !isDown)
            {
                Rack.NetychordsDMIBox.KeyDown = true;
                isDown = true;
                return 0;
            }
            if (e.VirtualKey == (ushort)space && e.KeyPressState == KeyPressState.Up)
            {
                Rack.NetychordsDMIBox.KeyDown = false;
                isDown = false;
                return 0;
            };
            return 1;
        }
    }
}
