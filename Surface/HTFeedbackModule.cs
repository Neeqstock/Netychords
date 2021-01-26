using NeeqDMIs.Headtracking.NeeqHT;
using Netytar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private readonly SolidColorBrush horLineStroke;
        private readonly SolidColorBrush verLineStroke;

        public HTFeedbackModes Mode { get; set; }

        public HTFeedbackModule(Canvas canvas, HTFeedbackModes mode)
        {
            Canvas = canvas;
            
            horLine = new Line();
            horLine.StrokeThickness = horLineThickness;
            horLine.Stroke = horLineStroke;
            canvas.Children.Add(horLine);

            verLine = new Line();
            verLine.StrokeThickness = verLineThickness;
            verLine.Stroke = verLineStroke;
            canvas.Children.Add(verLine);
            
            Mode = mode;
        }

        public void UpdateGraphics(HeadTrackerData headTrackerData, NetychordsButton lastCheckedButton)
        {
            horLine.X1 = Canvas.GetLeft(lastCheckedButton) + lastCheckedButton.ActualWidth / 2;
            horLine.Y1 = Canvas.GetTop(lastCheckedButton) + lastCheckedButton.ActualHeight / 2;
            
            horLine.X2 = Canvas.
        }

        public enum HTFeedbackModes
        {
            Bars,
            HalfButton,
            DeadZone
        }
    }
}
