using NeeqDMIs.Mouse;
using NeeqDMIs.Utils;

namespace HANDMIsTestSuite.Behaviors.Mouse
{
    public class BMouseX : AMouseBehavior
    {
        private ValueNormalizerDouble valueNormalizerX;

        public BMouseX()
        {
            ControlMode.Add(ControlModes.MouseX);
            valueNormalizerX = new ValueNormalizerDouble(System.Windows.SystemParameters.PrimaryScreenWidth, Rack.DMIBox.ScreenWidth);
        }
        public override bool ReceiveFire()
        {            
            if (IsActive())
            {
                Rack.DMIBox.XCursorCenter = (int)valueNormalizerX.Normalize(MouseFunctions.GetCursorPosition().X) - GlobalValues.HalfCursorWidth;
                if (Rack.DMIBox.PollingMode == Modules.Polling.PollingMode.Stopwatch)
                {
                    Rack.DMIBox.PollingModuleStopwatch.ReceiveStopwatchStamp();
                }
                return true;
            }
            return false;
        }
    }
}
