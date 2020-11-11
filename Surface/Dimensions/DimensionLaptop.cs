using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netytar
{
    class DimensionLaptop : IDimension
    {
        public int HighlightStrokeDim { get; } = 5;
        public int HighlightRadius { get; } = 65;
        public int VerticalSpacer { get; } = 100;
        public int HorizontalSpacer { get; } = 200;
        public int ButtonHeight { get; } = 13;
        public int ButtonWidth { get; } = 13;
        public int OccluderOffset { get; } = 25;
        public int EllipseStrokeDim { get; } = 15;
        public int EllipseStrokeSpacer { get; } = 15;
        public int LineThickness { get; } = 3;
    }
}
