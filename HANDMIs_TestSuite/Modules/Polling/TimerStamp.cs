namespace HANDMIsTestSuite.Modules.Polling
{
    public class TimerStamp
    {
        private long stampNumber;
        private int successCounter;
        private bool isSuccess;
        private int targetCenter;
        private int cursorCenter;
        private long timingErrorMicrosec;

        public TimerStamp(long stampNumber, int successCounter, bool isSuccess, int targetCenter, int cursorCenter, long timingErrorMicrosec)
        {
            this.stampNumber = stampNumber;
            this.SuccessCounter = successCounter;
            this.IsSuccess = isSuccess;
            this.TargetCenter = targetCenter;
            this.CursorCenter = cursorCenter;
            this.TimingErrorMicrosec = timingErrorMicrosec;
        }

        public long StampNumber { get => stampNumber; set => stampNumber = value; }
        public int SuccessCounter { get => successCounter; set => successCounter = value; }
        public bool IsSuccess { get => isSuccess; set => isSuccess = value; }
        public int TargetCenter { get => targetCenter; set => targetCenter = value; }
        public int CursorCenter { get => cursorCenter; set => cursorCenter = value; }
        public long TimingErrorMicrosec { get => timingErrorMicrosec; set => timingErrorMicrosec = value; }
    }
}
