using Tobii.Interaction;
using Tobii;

namespace NeeqDMIs.Eyetracking.Tobii
{
    public interface ITobiiHeadPoseBehavior
    {
        void ReceiveHeadPoseData(HeadPoseData data);
    }
}