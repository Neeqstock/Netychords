using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.Microphone
{
    public interface MicControllerListener
    {
        void processStartBlowing();
        void processStopBlowing();
    }
}
