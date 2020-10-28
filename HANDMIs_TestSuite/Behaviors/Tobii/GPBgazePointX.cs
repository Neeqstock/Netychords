using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Utils;
using Tobii.Interaction;

namespace HANDMIsTestSuite.Behaviors.Tobii
{
    public class GPBgazePointX : ITobiiGazePointBehavior
    {
        private ValueNormalizerDouble valueNormalizer;

        public GPBgazePointX()
        {
            valueNormalizer = new ValueNormalizerDouble(System.Windows.SystemParameters.PrimaryScreenWidth, Rack.DMIBox.ScreenWidth);
        }

        public void ReceiveGazePoint(GazePointData e)
        {
            if (Rack.DMIBox.ControlMode == ControlModes.GazePointXRaw && Rack.DMIBox.TestStarted)
            {
                Rack.DMIBox.XCursorCenter = (int)valueNormalizer.Normalize(e.X);
                Rack.DMIBox.Str_HeadTrackerRaw = ((int)e.X).ToString();
                //Rack.DMIBox.XCursorCenter = ((int)System.Windows.Input.Mouse.GetPosition(Rack.DMIBox.XCanvas).X);

                if (Rack.DMIBox.PollingMode == Modules.Polling.PollingMode.Stopwatch)
                {
                    Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
                }
            }
        }
    }
}
