using Tobii.Interaction;

namespace NeeqDMIs.Eyetracking.Tobii
{
    public interface ITobiiEyePositionBehavior
    {
        void ReceiveEyePositionData(EyePositionData e);
    }
}