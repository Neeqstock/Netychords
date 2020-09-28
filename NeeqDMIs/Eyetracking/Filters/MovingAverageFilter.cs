using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.Eyetracking.Filters
{
    /// <summary>
    /// An array-based moving average filter. Calculates the average of the last [arrayDimension] points.
    /// </summary>
    public class MovingAverageFilter : IPointFilter
    {
        private Point[] points;

        public MovingAverageFilter(int arrayDimension)
        {
            this.points = new Point[arrayDimension];
        }

        public void Push(Point point)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                points[i + 1] = points[i];
            }

            points[0] = point;
        }

        public Point GetOutput()
        {
            int x = 0;
            int y = 0;
            foreach (Point point in points)
            {
                x += (int)point.X;
                y += (int)point.Y;
            }
            x = (int)(x / points.Length);
            y = (int)(y / points.Length);
            return new Point(x, y);
        }
    }
}
