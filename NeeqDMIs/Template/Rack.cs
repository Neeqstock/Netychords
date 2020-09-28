using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs
{
    public static class Rack
    {
        private static DMIBox dmibox;
        public static DMIBox DMIBox { get => dmibox; set => dmibox = value; }
    }
}
