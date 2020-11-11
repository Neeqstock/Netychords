using NeeqDMIs.Music;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Netytar
{
    public class NetytarButton : RadioButton
    {
        #region Internal params
        private MidiNotes note;
        public MidiNotes Note { get { return note; } set { note = value; } }

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

        private NetytarSurface netytarDrawer;
        #endregion

        #region Own lines
        public Line L_p1
        {
            get
            {
                return l_p1;
            }

            set
            {
                l_p1 = value;
            }
        }
        public Line L_p2
        {
            get
            {
                return l_p2;
            }

            set
            {
                l_p2 = value;
            }
        }
        public Line L_p3
        {
            get
            {
                return l_p3;
            }

            set
            {
                l_p3 = value;
            }
        }
        public Line L_m1
        {
            get
            {
                return l_m1;
            }

            set
            {
                l_m1 = value;
            }
        }
        public Line L_m2
        {
            get
            {
                return l_m2;
            }

            set
            {
                l_m2 = value;
            }
        }
        public Line L_m3
        {
            get
            {
                return l_m3;
            }

            set
            {
                l_m3 = value;
            }
        }

        private Line l_p1;
        private Line l_p2;
        private Line l_p3;

        private Line l_m1;
        private Line l_m2;
        private Line l_m3;
        #endregion

        public NetytarButton(NetytarSurface netytarDrawer) : base()
        {
            this.netytarDrawer = netytarDrawer;

            occluder = new Rectangle();
            occluder.Stroke = Brushes.Transparent;
            occluder.Fill = Brushes.Transparent;
            occluder.Stroke = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
            occluder.Fill = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
            occluder.StrokeThickness = 1;
            occluder.HorizontalAlignment = HorizontalAlignment.Left;
            occluder.VerticalAlignment = VerticalAlignment.Center;

            occluder.MouseEnter += OccluderMouseEnterBehavior;
        }

        public Line getLine(int pitchDirection)
        {
            switch (pitchDirection)
            {
                case -1:
                    return l_m1;
                case -2:
                    return l_m2;
                case -3:
                    return l_m3;
                case 1:
                    return l_p1;
                case 2:
                    return l_p2;
                case 3:
                    return l_p3;
                default:
                    return null;
            }
        }

        private void OccluderMouseEnterBehavior(object sender, MouseEventArgs e)
        {
            netytarDrawer.NetytarButton_OccluderMouseEnter(this);
        }
    }
}
