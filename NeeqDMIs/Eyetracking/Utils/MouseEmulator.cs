using NeeqDMIs.Eyetracking.Filters;
using NeeqDMIs.Mouse;
using System.Drawing;

namespace NeeqDMIs.Eyetracking.Utils
{
    public class MouseEmulator
    {
        private IPointFilter filter;
        public IPointFilter Filter { get => filter; set => filter = value; }
        private Point currentGazePoint = new Point();

        private bool eyetrackerToMouse = false;
        private bool cursorVisible = true;

        public MouseEmulator(IPointFilter filter)
        {
            this.Filter = filter;
        }

        public bool EyetrackerToMouse
        {
            get { return eyetrackerToMouse; }
            set { eyetrackerToMouse = value; }
        }

        public bool CursorVisible
        {
            get { return cursorVisible; }
            set
            {
                cursorVisible = value;
                MouseFunctions.ShowCursor(cursorVisible);
            }
        }

        public void ReceiveGazePointData(double X, double Y)
        {
            if (eyetrackerToMouse)
            {
                currentGazePoint.X = (int)X;
                currentGazePoint.Y = (int)Y;

                Filter.Push(currentGazePoint);

                MouseFunctions.SetCursorPos(Filter.GetOutput().X, Filter.GetOutput().Y);
            }
        }
    }
}
