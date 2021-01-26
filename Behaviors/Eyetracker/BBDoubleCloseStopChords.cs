using NeeqDMIs.Eyetracking.Tobii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netytar.Behaviors.Eyetracker
{
    class BBDoubleCloseStopChords : ATobiiBlinkBehavior
    {
        public override void Event_doubleClose()
        {
            Rack.NetychordsDMIBox.StopChord(Rack.NetychordsDMIBox.Chord);
        }

        public override void Event_doubleOpen()
        {
        }

        public override void Event_leftClose()
        {
        }

        public override void Event_leftOpen()
        {
        }

        public override void Event_rightClose()
        {
        }

        public override void Event_rightOpen()
        {
        }
    }
}
