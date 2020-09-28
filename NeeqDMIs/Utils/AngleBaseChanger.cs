namespace NeeqDMIs.Utils
{
    /// <summary>
    /// Does your gyroscope or whatever return you angles in [-180; +180] format, and you want to move the center wherever you desire?
    /// Here I am!
    /// </summary>
    public class AngleBaseChanger
    {
        private double deltaBar = 180;
        private double delta = 0;

        /// <summary>
        /// Defines the new rotated base. Delta is the angle that your sensor read when your gyro is in the desired center.
        /// </summary>
        public double Delta
        {
            get { return delta; }
            set
            {
                delta = value;
                if (value >= 0)
                {
                    deltaBar = -180 + value;
                }
                else
                {
                    deltaBar = 180 + value;
                }
            }
        }

        public double getDeltaBar() //DEBUG PURPOSES
        {
            return deltaBar;
        }

        /// <summary>
        /// Converts a sensor read into the new base defined by delta.
        /// </summary>
        /// <param name="angle">The angle read by the sensor</param>
        /// <returns></returns>
        public double Transform(double angle)
        {

            double res = 0;
            if (delta >= 0)
            {
                if(angle > deltaBar)
                {
                    res = angle - delta;
                }
                else
                {
                    res = 180 + angle - deltaBar;
                }
            }
            else if (delta < 0)
            {
                if(angle < deltaBar)
                {
                    res = angle - delta;
                }
                else
                {
                    res = -180 + angle - deltaBar;
                }
            }
            return res;
        }





    }
}
