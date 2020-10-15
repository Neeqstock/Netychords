using NeeqDMIs.Music;
using Netytar.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Netytar
{
    public class NetychordsButton : RadioButton
    {
        #region Internal params
        private MidiChord chord;
        public MidiChord Chord { get { return chord; } set { chord = value; } }

        private Rectangle occluder;
        public Rectangle Occluder
        {
            get
            {
                return occluder;
            }

            set
            {
                occluder = value;
            }
        }

        private NetychordsSurface NetychordsDrawer;
        #endregion

        public NetychordsButton(NetychordsSurface NetychordsDrawer) : base()
        {
            this.NetychordsDrawer = NetychordsDrawer;

            occluder = new Rectangle();
            occluder.Stroke = Brushes.Transparent;
            occluder.Fill = Brushes.Transparent;
            occluder.Stroke = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
            occluder.Fill = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0));
            occluder.StrokeThickness = 1;
            occluder.HorizontalAlignment = HorizontalAlignment.Left;
            occluder.VerticalAlignment = VerticalAlignment.Center;

            occluder.MouseEnter += OccluderMouseEnterBehavior;
        }

        private void OccluderMouseEnterBehavior(object sender, MouseEventArgs e)
        {
            NetychordsDrawer.NetychordsButton_OccluderMouseEnter(this);
        }
    }
}
