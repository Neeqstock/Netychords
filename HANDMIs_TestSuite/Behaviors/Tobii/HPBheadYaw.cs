using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Utils;
using Tobii.Interaction;

namespace HANDMIsTestSuite.Behaviors.Tobii
{
    public class HPBheadYaw : ITobiiHeadPoseBehavior
    {
        private ValueNormalizerDouble valueNormalizer;
        private double value;
        private double bound;

        public HPBheadYaw(double bound)
        {
            this.bound = bound;
            valueNormalizer = new ValueNormalizerDouble(GlobalValues.BarWidth, bound * 2);
        }

        public void ReceiveHeadPoseData(HeadPoseData data)
        {
            if (Rack.DMIBox.ControlMode == ControlModes.HeadYaw && Rack.DMIBox.TestStarted)
            {
                
                // Rack.DMIBox.XCursorCenter = (int)valueNormalizer.Normalize(e.X);
                value = -data.HeadRotation.Y + bound;

                value = valueNormalizer.Normalize(value);
                Rack.DMIBox.Str_HeadTrackerRaw = value.ToString();
                Rack.DMIBox.XCursorValue = (int)value;



                //Rack.DMIBox.XCursorCenter = ((int)System.Windows.Input.Mouse.GetPosition(Rack.DMIBox.XCanvas).X);

                if (Rack.DMIBox.PollingMode == Modules.Polling.PollingMode.Stopwatch)
                {
                    Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
                }
            }
        }
    }
}
