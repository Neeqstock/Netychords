using System.Collections.Generic;
using System.Windows.Media;

namespace Netytar
{
    class ColorCodeStandard : IColorCode
    {
        public List<Color> KeysColorCode { get; } = new List<Color>()
        {
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.LightGreen,
            Colors.Blue,
            Colors.Purple,
            Colors.Coral
        };
        public SolidColorBrush NotInScaleBrush { get; } = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));
        public SolidColorBrush MinorBrush { get; } = new SolidColorBrush(Colors.Blue);
        public SolidColorBrush MajorBrush { get; } = new SolidColorBrush(Colors.Red);
        public SolidColorBrush HighlightBrush { get; } = new SolidColorBrush(Colors.White);
        public SolidColorBrush TransparentBrush { get; } = new SolidColorBrush(Colors.Transparent);
    }
}
