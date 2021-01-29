using NeeqDMIs.Headtracking.NeeqHT;
using NeeqDMIs.Utils;
using Netytar;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Eyerpheus.Controllers.Graphics
{
    public class HTFeedbackModule
    {
        private const int Bar_horLineThickness = 4;
        private const int Bar_verLineThickness = 4;
        private const int Half_midLineThickness = 5;
        private readonly SolidColorBrush Bar_horLineStroke = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Bar_verLineStroke = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Half_leftRectColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Half_midLineColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush Half_rightRectColor = new SolidColorBrush(Colors.White);
        private Line Bar_horLine;
        private Line Bar_verLine;
        private double Bar_verLineHeight = 50;
        private Canvas Canvas;
        private Rectangle Half_leftRect;
        private Line Half_midLine;
        private ValueMapperDouble Half_rectRangeMapper = new ValueMapperDouble(80, 1);
        private Rectangle Half_rightRect;
        private double Half_VLHeight;
        public HTFeedbackModes Mode { get; set; }

        public HTFeedbackModule(Canvas canvas, HTFeedbackModes mode)
        {
            Canvas = canvas;
            Mode = mode;

            switch (mode)
            {
                case HTFeedbackModes.Bars:
                    Bar_horLine = new Line();
                    Bar_horLine.StrokeThickness = Bar_horLineThickness;
                    Bar_horLine.Stroke = Bar_horLineStroke;
                    Panel.SetZIndex(Bar_horLine, 2000);
                    canvas.Children.Add(Bar_horLine);

                    Bar_verLine = new Line();
                    Bar_verLine.StrokeThickness = Bar_verLineThickness;
                    Bar_verLine.Stroke = Bar_verLineStroke;
                    Panel.SetZIndex(Bar_verLine, 2000);
                    canvas.Children.Add(Bar_verLine);

                    Half_VLHeight = Bar_verLineHeight / 2;

                    break;

                case HTFeedbackModes.HalfButton:
                    Half_leftRect = new Rectangle();
                    Half_leftRect.Fill = Half_leftRectColor;
                    Half_leftRect.Opacity = 0;
                    Panel.SetZIndex(Half_leftRect, 2000);
                    canvas.Children.Add(Half_leftRect);

                    Half_rightRect = new Rectangle();
                    Half_rightRect.Fill = Half_rightRectColor;
                    Half_rightRect.Opacity = 0;
                    Panel.SetZIndex(Half_rightRect, 2000);
                    canvas.Children.Add(Half_rightRect);

                    Half_midLine = new Line();
                    Half_midLine.Stroke = Half_midLineColor;
                    Half_midLine.StrokeThickness = Half_midLineThickness;
                    Half_midLine.Opacity = 0;
                    Panel.SetZIndex(Half_midLine, 2001);
                    canvas.Children.Add(Half_midLine);

                    break;

                case HTFeedbackModes.DeadZone:
                    break;
            }
        }

        public void UpdateGraphics(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            switch (Mode)
            {
                case HTFeedbackModes.Bars:
                    Update_Bars(headTrackerData, checkedButton);
                    break;

                case HTFeedbackModes.HalfButton:
                    Update_HalfButton(headTrackerData, checkedButton);
                    break;

                case HTFeedbackModes.DeadZone:
                    Update_DeadZone(headTrackerData, checkedButton);
                    break;
            }
        }

        #region Update resolvers

        private void Update_Bars(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                Bar_horLine.X1 = Canvas.GetLeft(checkedButton) + checkedButton.ActualWidth / 2;
                Bar_horLine.Y1 = Canvas.GetTop(checkedButton) + checkedButton.ActualHeight / 2;

                Bar_horLine.X2 = Bar_horLine.X1 + headTrackerData.TranspYaw;
                Bar_horLine.Y2 = Bar_horLine.Y1;

                Bar_verLine.X1 = Bar_horLine.X2;
                Bar_verLine.Y1 = Bar_horLine.Y2 - Half_VLHeight;

                Bar_verLine.X2 = Bar_horLine.X2;
                Bar_verLine.Y2 = Bar_horLine.Y2 + Half_VLHeight;

                Canvas.UpdateLayout();
            }
        }

        private void Update_DeadZone(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
        }

        private void Update_HalfButton(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                var Occ = checkedButton.Occluder;

                Half_leftRect.Height = Occ.ActualHeight;
                Half_leftRect.Width = Occ.ActualWidth / 2;

                Half_rightRect.Height = Occ.ActualHeight;
                Half_rightRect.Width = Occ.ActualWidth / 2;

                Half_midLine.X1 = Canvas.GetLeft(Occ) + Occ.ActualWidth / 2;
                Half_midLine.Y1 = Canvas.GetTop(Occ);
                Half_midLine.X2 = Canvas.GetLeft(Occ) + Occ.ActualWidth / 2;
                Half_midLine.Y2 = Canvas.GetTop(Occ) + Occ.ActualHeight;

                Canvas.SetLeft(Half_leftRect, Canvas.GetLeft(Occ));
                Canvas.SetLeft(Half_rightRect, Canvas.GetLeft(Occ) + Occ.ActualWidth / 2);
                Canvas.SetTop(Half_leftRect, Canvas.GetTop(Occ));
                Canvas.SetTop(Half_rightRect, Canvas.GetTop(Occ));

                double posPart = 0;
                double negPart = 0;

                if (headTrackerData.TranspYaw >= 0)
                {
                    posPart = headTrackerData.TranspYaw;
                    negPart = 0;
                }
                else
                {
                    negPart = -headTrackerData.TranspYaw;
                    posPart = 0;
                }

                if (Rack.NetychordsDMIBox.InDeadZone)
                {
                    Half_midLine.Opacity = 1;
                }
                else
                {
                    Half_midLine.Opacity = 0;
                }

                Half_leftRect.Opacity = Half_rectRangeMapper.Map(negPart);
                Half_rightRect.Opacity = Half_rectRangeMapper.Map(posPart);
            }
        }

        #endregion Update resolvers

        public enum HTFeedbackModes
        {
            Bars,
            HalfButton,
            DeadZone
        }
    }
}