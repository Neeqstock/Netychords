using System.Drawing;

namespace NeeqDMIs.Eyetracking.Filters
{
    /// <summary>
    /// A filter which does... Nothing! Output = input.
    /// </summary>
    public class NoFilter : IPointFilter
    {
        private Point point;

        public NoFilter()
        {
            this.point = new Point(0,0);
        }

        public void Push(Point point)
        {
            this.point = point;
        }

        public Point GetOutput()
        {
            return point;
        }
    }
}
