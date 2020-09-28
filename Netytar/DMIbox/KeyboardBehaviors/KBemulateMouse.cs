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
            switch (Rack.NetytarDMIBox.Eyetracker)
            {

                case Eyetracker.Tobii:
                    Rack.NetytarDMIBox.TobiiModule.MouseEmulator.EyetrackerToMouse = eyeTrackerToMouse;
                    Rack.NetytarDMIBox.TobiiModule.MouseEmulator.CursorVisible = cursorVisible;
                    Rack.NetytarDMIBox.AutoScroller.Enabled = autoScrollerEnabled;
                    break;
                case Eyetracker.Eyetribe:
                    Rack.NetytarDMIBox.EyeTribeModule.MouseEmulator.EyetrackerToMouse = eyeTrackerToMouse;
                    Rack.NetytarDMIBox.EyeTribeModule.MouseEmulator.CursorVisible = cursorVisible;
                    Rack.NetytarDMIBox.AutoScroller.Enabled = autoScrollerEnabled;
                    break;
                default:
                    break;
            }
        }
    }
}
