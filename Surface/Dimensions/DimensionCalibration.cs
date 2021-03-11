using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netychords
{
    class DimensionCalibration : IDimension
    {
        public int HighlightStrokeDim { get; } = 5;
        public int HighlightRadius { get; } = 65;
        public int VerticalSpacer { get; } = 0;
        public int HorizontalSpacer { get; } = 0;
        public int ButtonHeight { get; } = 23;
        public int ButtonWidth { get; } = 23;
        public int OccluderOffset { get; } = 38;
        public int EllipseStrokeDim { get; } = 15;
        public int EllipseStrokeSpacer { get; } = 15;
        public int LineThickness { get; } = 3;
    }
}
