using EyeTribe.ClientSdk.Data;

namespace NeeqDMIs.Eyetracking.Eyetribe
{
    public interface IEyeTribeGazePointBehavior
    {
        void ReceiveGazePoint(GazeData e);
    }
}
