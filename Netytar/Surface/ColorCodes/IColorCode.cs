using System.Collections.Generic;
using System.Windows.Media;

namespace Netytar
{
    public interface IColorCode
    {
        List<Color> KeysColorCode { get; }
        SolidColorBrush NotInScaleBrush { get; }
        SolidColorBrush MinorBrush { get; }
        SolidColorBrush MajorBrush { get; }
        SolidColorBrush HighlightBrush { get; }
        SolidColorBrush TransparentBrush { get; }
    }
}
