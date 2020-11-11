using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HANDMIs_TestSuite.Utils
{
    public class TargetDistance
    {
        public TargetDistance(long distance)
        {
            Distance = distance;
        }

        public TargetDistance()
        {
        }

        public long Distance { get; set; }
    }
}
