namespace NeeqDMIs.Eyetracking.Tobii
{
    public abstract class ATobiiBlinkBehavior
    {
        #region Thresholds
        public const int DefaultThresh = 30;

        private int lOThresh = DefaultThresh;
        private int lCThresh = DefaultThresh;
        private int rOThresh = DefaultThresh;
        private int rCThresh = DefaultThresh;
        private int dOThresh = DefaultThresh;
        private int dCThresh = DefaultThresh;

        public int LOThresh { get => lOThresh; set => lOThresh = value; }
        public int LCThresh { get => lCThresh; set => lCThresh = value; }
        public int ROThresh { get => rOThresh; set => rOThresh = value; }
        public int RCThresh { get => rCThresh; set => rCThresh = value; }
        public int DOThresh { get => dOThresh; set => dOThresh = value; }
        public int DCThresh { get => dCThresh; set => dCThresh = value; }
        #endregion

        public abstract void Event_leftOpen();
        public abstract void Event_leftClose();
        public abstract void Event_doubleOpen();
        public abstract void Event_doubleClose();
        public abstract void Event_rightClose();
        public abstract void Event_rightOpen();

        public void ReceiveCounters(int leftOpenCounter, int leftCloseCounter, int rightOpenCounter, int rightCloseCounter, int doubleOpenCounter, int doubleCloseCounter)
        {
            if (leftOpenCounter == LOThresh)
            {
                Event_leftOpen();
            }
            if (leftCloseCounter == LCThresh)
            {
                Event_leftClose();
            }
            if (rightOpenCounter == ROThresh)
            {
                Event_rightOpen();
            }
            if (rightCloseCounter == RCThresh)
            {
                Event_rightClose();
            }
            if (doubleOpenCounter == DOThresh)
            {
                Event_doubleOpen();
            }
            if (doubleCloseCounter == DCThresh)
            {
                Event_doubleClose();
            }
        }
    }
}
