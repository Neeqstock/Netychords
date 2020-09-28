using RawInputProcessor;

namespace NeeqDMIs.Keyboard
{
    public abstract class AKeyboardBehavior
    {
        public abstract int ReceiveEvent(RawInputEventArgs e);
    }
}