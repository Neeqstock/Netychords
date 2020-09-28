using System.Drawing;
using System.Runtime.InteropServices;

namespace NeeqDMIs.Mouse
{
    public static class MouseFunctions
    {
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32")]
        public static extern int ShowCursor(bool bShow);

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        private static POINT lpPoint;
        public static Point GetCursorPosition()
        {
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            //if (!success)
            return lpPoint;
        }
    }
}