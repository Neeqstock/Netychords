using NeeqDMIs.Headtracking.NeeqHT;
using Netytar;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Eyerpheus.Controllers.Graphics
{
    public class HTFeedbackModule
    {
        public Canvas Canvas;
        public double CenterX;
        public double CenterY;
        public Line horLine;
        public Line verLine;

        private const int horLineThickness = 4;
        private const int verLineThickness = 4;
        private readonly SolidColorBrush horLineStroke = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush verLineStroke = new SolidColorBrush(Colors.White);
        private double verLineHeight = 50;
        private double halfVLHeight;

        private HTFeedbackModes Mode { get; set; }

        public HTFeedbackModule(Canvas canvas, HTFeedbackModes mode)
        {
            Canvas = canvas;

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

            Mode = mode;
        }

        public void UpdateGraphics(HeadTrackerData headTrackerData, NetychordsButton checkedButton)
        {
            switch (Mode)
            {
                case HTFeedbackModes.Bars:
                    Update_Bars(headTrackerData, checkedButton);
                    break;
                case HTFeedbackModes.HalfButton:
                    break;
                case HTFeedbackModes.DeadZone:
                    break;
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
