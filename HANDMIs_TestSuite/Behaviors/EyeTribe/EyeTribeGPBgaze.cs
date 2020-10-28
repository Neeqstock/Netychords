using EyeTribe.ClientSdk.Data;
using HANDMIsTestSuite.Modules.Polling;
using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Utils;

namespace HANDMIsTestSuite.Behaviors.EyeTribe
{
    public class EyeTribeGPBgaze : IEyeTribeGazePointBehavior
    {
        public GazeMode mode { get; set; }

        double gX = 0;
        double gY = 0;

        private ValueNormalizerDouble valueNormalizerX;
        private ValueNormalizerDouble valueNormalizerY;

        public EyeTribeGPBgaze(GazeMode mode)
        {
            this.mode = mode;
            valueNormalizerX = new ValueNormalizerDouble(System.Windows.SystemParameters.PrimaryScreenWidth, Rack.DMIBox.ScreenWidth);
            valueNormalizerY = new ValueNormalizerDouble(System.Windows.SystemParameters.PrimaryScreenHeight, Rack.DMIBox.ScreenHeight);
        }

        public void ReceiveGazePoint(GazeData e)
        {
            if (e.HasRawGazeCoordinates())
            {
                Rack.DMIBox.EyeTribeGPData = e;

                if ((Rack.DMIBox.ControlMode == ControlModes.GazePointXRaw || Rack.DMIBox.ControlMode == ControlModes.GazePointXSmooth) && Rack.DMIBox.TestStarted)
                {
                    if (Rack.DMIBox.ControlMode == ControlModes.GazePointXRaw)
                    {
                        gX = e.RawCoordinates.X;
                        gY = e.RawCoordinates.Y;
                    }
                    else if (Rack.DMIBox.ControlMode == ControlModes.GazePointXSmooth)
                    {
                        gX = e.SmoothedCoordinates.X;
                        gY = e.SmoothedCoordinates.Y;
                    }

                    Rack.DMIBox.XCursorCenter = (int)valueNormalizerX.Normalize(gX) - GlobalValues.HalfCursorWidth;

                    if (Rack.DMIBox.PollingMode == PollingMode.Stopwatch)
                    {
                        Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
                    }
                }
            }
        }
    }
}
