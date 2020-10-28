using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using RawInputProcessor;

namespace Netytar.DMIBox.KeyboardBehaviors
{
    class KBselectScale : AKeyboardBehavior
    {
        private const VKeyCodes keyMaj = VKeyCodes.Add;
        private const VKeyCodes keyMin = VKeyCodes.Subtract;

        public override int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyMaj && e.KeyPressState == KeyPressState.Down)
            {
                Rack.NetytarDMIBox.NetytarSurface.Scale = new Scale(Rack.NetytarDMIBox.NetytarSurface.CheckedButton.Note.ToAbsNote(), ScaleCodes.maj);
                return 1;
            }
            if (e.VirtualKey == (ushort)keyMaj && e.KeyPressState == KeyPressState.Up)
            {
                Rack.NetytarDMIBox.NetytarSurface.Scale = new Scale(Rack.NetytarDMIBox.NetytarSurface.CheckedButton.Note.ToAbsNote(), ScaleCodes.min);
            };
            return 0;
        }
    }
}
