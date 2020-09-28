using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.Eyetracking.Filters
{
    /// <summary>
    /// Outputs the average of the last [arrayDimension] points, but discards the "outsiders": namely, those points for which the difference with the last calculated mean exceeds [maxVariation]. Can increase the stability of a noisy signal.
    /// </summary>
    public class MASelectiveFilter : IPointFilter
    {
        private Point[] points;
        private Point lastMean = new Point(0, 0);
        private int maxVariation;

        public MASelectiveFilter(int arrayDimension, int maxVariation)
        {
            this.points = new Point[arrayDimension];
            this.maxVariation = maxVariation;
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
                if (Math.Abs(x - lastMean.X) < maxVariation)
                {
                    x += (int)point.X;
                }
                if (Math.Abs(y - lastMean.Y) < maxVariation)
                {
                    y += (int)point.Y;
                }
            }
            x = (int)(x / points.Length);
            y = (int)(y / points.Length);
            this.lastMean.X = x;
            this.lastMean.Y = y;
            return new Point(x, y);
        }
    }
}
