namespace HANDMIsTestSuite.Modules.Polling
{
    public class StopWatchStamp
    {
        public StopWatchStamp(long stampNumber, LockedStates lockedState, int targetCenter, int cursorCenter, long timeInMicroseconds)
        {
            this.StampNumber = stampNumber;
            this.LockedState = lockedState;
            this.TargetCenter = targetCenter;
            this.CursorCenter = cursorCenter;
            this.TimeInMicroseconds = timeInMicroseconds;
        }

        public long StampNumber { get; set; }
        public LockedStates LockedState { get; set; }
        public int TargetCenter { get; set; }
        public int CursorCenter { get; set; }
        public long TimeInMicroseconds { get; set; }
    }
}
