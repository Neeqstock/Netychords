using NeeqDMIs.Headtracking.NeeqHT;
using NeeqDMIs.Utils;
using Netytar;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Eyerpheus.Controllers.Graphics
{
    public class HTFeedbackModule
    {
        private Canvas Canvas;
        private Line horLine;
        private Line verLine;


        private const int horLineThickness = 4;
        private const int verLineThickness = 4;
        private readonly SolidColorBrush horLineStroke = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush verLineStroke = new SolidColorBrush(Colors.White);
        private double verLineHeight = 50;
        private double halfVLHeight;

        private Rectangle leftRect;
        private Rectangle rightRect;
        private readonly SolidColorBrush leftRectColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush rightRectColor = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush deadZoneColor = new SolidColorBrush(Colors.White);
        private ValueMapperDouble rectRangeMapper = new ValueMapperDouble(80, 1);
        private Line centerRect;

        public HTFeedbackModes Mode { get; set; }

        public HTFeedbackModule(Canvas canvas, HTFeedbackModes mode)
        {
            Canvas = canvas;
            Mode = mode;

            switch (mode)
            {
                case HTFeedbackModes.Bars:
                    horLine = new Line();
                    horLine.StrokeThickness = horLineThickness;
                    horLine.Stroke = horLineStroke;
                    Panel.SetZIndex(horLine, 2000);
                    canvas.Children.Add(horLine);

                    verLine = new Line();
                    verLine.StrokeThickness = verLineThickness;
                    verLine.Stroke = verLineStroke;
                    Panel.SetZIndex(verLine, 2000);
                    canvas.Children.Add(verLine);

                    halfVLHeight = verLineHeight / 2;

                    break;
                
                case HTFeedbackModes.HalfButton:
                    leftRect = new Rectangle();
                    leftRect.Fill = leftRectColor;
                    leftRect.Opacity = 0; 
                    Panel.SetZIndex(leftRect, 2000);
                    canvas.Children.Add(leftRect);

                    rightRect = new Rectangle();
                    rightRect.Fill = rightRectColor;
                    rightRect.Opacity = 0;
                    Panel.SetZIndex(rightRect, 2000);
                    canvas.Children.Add(rightRect);



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

        private void Update_DeadZone(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            
        }

        private void Update_HalfButton(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                var Occ = checkedButton.Occluder;

                leftRect.Height = Occ.ActualHeight;
                leftRect.Width = Occ.ActualWidth / 2;

                rightRect.Height = Occ.ActualHeight;
                rightRect.Width = Occ.ActualWidth / 2;

                Canvas.SetLeft(leftRect, Canvas.GetLeft(Occ));
                Canvas.SetLeft(rightRect, Canvas.GetLeft(Occ) + Occ.ActualWidth / 2);
                Canvas.SetTop(leftRect, Canvas.GetTop(Occ));
                Canvas.SetTop(rightRect, Canvas.GetTop(Occ));

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

                leftRect.Opacity = rectRangeMapper.Map(negPart);
                rightRect.Opacity = rectRangeMapper.Map(posPart);
            }
        }

        private void Update_Bars(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            if (checkedButton != null)
            {
                horLine.X1 = Canvas.GetLeft(checkedButton) + checkedButton.ActualWidth / 2;
                horLine.Y1 = Canvas.GetTop(checkedButton) + checkedButton.ActualHeight / 2;

                horLine.X2 = horLine.X1 + headTrackerData.TranspYaw;
                horLine.Y2 = horLine.Y1;

                verLine.X1 = horLine.X2;
                verLine.Y1 = horLine.Y2 - halfVLHeight;

                verLine.X2 = horLine.X2;
                verLine.Y2 = horLine.Y2 + halfVLHeight;

                Canvas.UpdateLayout();
            }
        }

        public enum HTFeedbackModes
        {
            Bars,
            HalfButton,
            DeadZone
        }
    }
}
