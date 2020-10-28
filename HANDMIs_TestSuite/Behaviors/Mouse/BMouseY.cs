namespace HANDMIsTestSuite.Behaviors.Mouse
{
    public class BMouseY : AMouseBehavior
    {
        public BMouseY()
        {
            ControlMode.Add(ControlModes.MouseY);
        }
        public override bool ReceiveFire()
        {
            if (IsActive())
            {
                //Rack.DMIBox.YCursorCenter = ((int)System.Windows.Input.Mouse.GetPosition(Rack.DMIBox.YCanvas).Y);
                return true;
            }
            return false;
        }
    }
}
