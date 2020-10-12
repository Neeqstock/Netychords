using NeeqDMIs.Keyboard;
using RawInputProcessor;
using System.Windows;

namespace Netytar
{
    class KBemulateMouse : AKeyboardBehavior
    {
        private VKeyCodes keyStartEmulate = VKeyCodes.Control;
        private VKeyCodes keyStopEmulate = VKeyCodes.Shift;

        private bool eyeTrackerToMouse = false;
        private bool cursorVisible = true;
        private bool autoScrollerEnabled = false;

        public override int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyStartEmulate)
            {
                eyeTrackerToMouse = true;
                cursorVisible = false;
                autoScrollerEnabled = true;

                SetStuff();

                return 0;
            }
            else if (e.VirtualKey == (ushort)keyStopEmulate)
            {
                eyeTrackerToMouse = false;
                cursorVisible = true;
                autoScrollerEnabled = false;

                SetStuff();

                return 0;
            }
            return 1;
        }

        private void SetStuff()
        {
            switch (Rack.NetychordsDMIBox.Eyetracker)
            {

                case Eyetracker.Tobii:
                    Rack.NetychordsDMIBox.TobiiModule.MouseEmulator.EyetrackerToMouse = eyeTrackerToMouse;
                    Rack.NetychordsDMIBox.TobiiModule.MouseEmulator.CursorVisible = cursorVisible;
                    Rack.NetychordsDMIBox.AutoScroller.Enabled = autoScrollerEnabled;
                    break;
                case Eyetracker.Eyetribe:
                    Rack.NetychordsDMIBox.EyeTribeModule.MouseEmulator.EyetrackerToMouse = eyeTrackerToMouse;
                    Rack.NetychordsDMIBox.EyeTribeModule.MouseEmulator.CursorVisible = cursorVisible;
                    Rack.NetychordsDMIBox.AutoScroller.Enabled = autoScrollerEnabled;
                    break;
                default:
                    break;
            }
        }
    }
}
