using Tobii.Interaction;

namespace NeeqDMIs.Eyetracking.Tobii
{
    public interface ITobiiGazePointBehavior
    {
        void ReceiveGazePoint(GazePointData e);
    }
}