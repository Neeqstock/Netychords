using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netytar
{
    public interface IDimension
    {
        int VerticalSpacer { get; }
        int HorizontalSpacer { get; }
        int ButtonHeight { get; }
        int ButtonWidth { get; }
        int OccluderOffset { get; }
        int EllipseStrokeDim { get; }
        int EllipseStrokeSpacer { get; }
        int LineThickness { get; }
        int HighlightStrokeDim { get; }
        int HighlightRadius { get; }

    }
}
