using System;
using System.Drawing;

namespace NeeqDMIs.Mathematics
{
    public class ExpDecayingFilter : IFilter
    {
        private double numI = 0f;
        private double numIplusOne = 0f;
        private float alpha;

        /// <summary>
        /// The classic implementation of an exponentially decaying moving average filter. 
        /// </summary>
        /// <param name="alpha">Indicates the speed of decreasing priority of the old values.</param>
        public ExpDecayingFilter(float alpha)
        {
            this.alpha = alpha;
        }

        public void Push(double number)
        {
            numI = numIplusOne;
   
            numIplusOne = alpha * number + (1 - alpha) * numI;
        }

        public double GetOutput()
        {
            
            return numIplusOne;
        }
    }
}
