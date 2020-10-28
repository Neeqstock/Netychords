using System.Collections.Generic;

namespace HANDMIsTestSuite.Behaviors
{
    public abstract class ABehavior
    {
        public List<ControlModes> ControlMode { get; set; } = new List<ControlModes>();

        public bool IsActive()
        {
            if (ControlMode.Contains(Rack.DMIBox.ControlMode) && Rack.DMIBox.TestStarted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}