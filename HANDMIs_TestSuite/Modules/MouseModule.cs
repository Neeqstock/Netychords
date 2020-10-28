using HANDMIsTestSuite.Behaviors.Mouse;
using MicroLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HANDMIsTestSuite.MouseSpace
{
    public class MouseModule
    {
        private MicroTimer fireTimer;

        public List<AMouseBehavior> MouseBehaviors { get; set; } = new List<AMouseBehavior>();

        public MouseModule(double interval)
        {
            fireTimer = new MicroTimer();
            fireTimer.Interval = 1000;
            fireTimer.MicroTimerElapsed += fireTimer_Tick;
        }

        private void fireTimer_Tick(object sender, MicroTimerEventArgs timerEventArgs)
        {
            try
            {
                foreach (AMouseBehavior v in MouseBehaviors)
                {
                    v.ReceiveFire();
                }
            }
            catch
            {

            }

        }

        public void Start()
        {
            fireTimer.Start();
        }

        public void Stop()
        {
            fireTimer.Stop();
        }
    }
}
