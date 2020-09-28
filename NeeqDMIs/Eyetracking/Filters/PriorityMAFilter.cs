using System;
using System.Collections.Generic;
using System.Drawing;

namespace NeeqDMIs.Eyetracking.Filters
{
    public enum DecreasingFunction
    {
        linear,
        quadratic
    };

    /// <summary>
    /// An array-based moving average filter. The "priority" of the points in the array decreases at each step, following a (selectable) linear or quadratic function. //TODO: quadratic still NOT implemented!
    /// </summary>
    public class PriorityMAFilter : IPointFilter
    {
        private List<KeyValuePair<Point, int>> pointsWithPriority = new List<KeyValuePair<Point, int>>();
        private int numberOfPoints;
        private DecreasingFunction function;
        private int weigths = 0;
        
        public PriorityMAFilter(int numberOfPoints, DecreasingFunction function)
        {
            this.numberOfPoints = numberOfPoints;
            this.function = function;

            for (int i = 0; i < numberOfPoints; i++)
            {
                pointsWithPriority.Add(new KeyValuePair<Point, int>(new Point(0, 0), numberOfPoints));

            }

            for (int i = 0; i < numberOfPoints; i++)
            {
                weigths += i;
            }

            Console.WriteLine(weigths);

        }

        public void Push(Point point)
        {
            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                pointsWithPriority[i+1] = new KeyValuePair<Point, int>(pointsWithPriority[i].Key, pointsWithPriority[i].Value - 1);
            }
        }

        public Point GetOutput()
        {
            Point weightedMean = new Point(0, 0);

            if (function == DecreasingFunction.linear)
            {

                for (int i = 0; i < numberOfPoints; i++)
                {
                    weightedMean.X += pointsWithPriority[i].Key.X * pointsWithPriority[i].Value;
                    weightedMean.Y += pointsWithPriority[i].Key.Y * pointsWithPriority[i].Value;
                }

                weightedMean.X = weightedMean.X / weigths;
                weightedMean.Y = weightedMean.X / weigths;

            }
            else
            {
                throw new NotImplementedException("Quadratic decreasing function for PriorityMAFilter has not been implemented yet...");
            }

            return weightedMean;            

        }
    }
}
