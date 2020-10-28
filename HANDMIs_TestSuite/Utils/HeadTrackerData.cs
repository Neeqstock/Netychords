using NeeqDMIs.Utils;

namespace HANDMIs_TestSuite.Utils
{
    public class HeadTrackerData
    {
        private AngleBaseChanger pitchTransf;
        private AngleBaseChanger yawTransf;
        private AngleBaseChanger rollTransf;
        public double Pitch { get; set; }
        public double Yaw { get; set; }
        public double Roll { get; set; }
        public double TranspPitch { get { return pitchTransf.Transform(Pitch); } }
        public double TranspYaw { get { return yawTransf.Transform(Yaw); } }
        public double TranspRoll { get { return rollTransf.Transform(Roll); } }
        public double Velocity { get; set; }

        public HeadTrackerData()
        {
            pitchTransf = new AngleBaseChanger();
            yawTransf = new AngleBaseChanger();
            rollTransf = new AngleBaseChanger();
        }

        public double GetYawDeltaBar()
        {
            return yawTransf.getDeltaBar();
        }

        public void SetDeltaForAll()
        {
            pitchTransf.Delta = Pitch;
            yawTransf.Delta = Yaw;
            rollTransf.Delta = Roll;
            //MessageBox.Show(yawTransf.Delta.ToString(CultureInfo.InvariantCulture) + "\n" + yawTransf.getDeltaBar());
        }

        public void SetPitchDelta()
        {
            pitchTransf.Delta = Pitch;
        }

        public void SetYawDelta()
        {
            yawTransf.Delta = Yaw;
        }

        public void SetRollDelta()
        {
            rollTransf.Delta = Roll;
        }
    }
}
