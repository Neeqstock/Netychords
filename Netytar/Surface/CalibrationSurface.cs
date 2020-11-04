using NeeqDMIs.Music;
using Netytar.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace Netytar
{
   
    public class CalibrationSurface
    {
        public Ellipse highlighter = new Ellipse(); //before was private

        #region Settings
        private List<Color> keysColorCode = new List<Color>()
        {
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.LightGreen,
            Colors.Blue,
            Colors.Purple,
            Colors.Coral
        };

        private SolidColorBrush notInScaleBrush;
        private SolidColorBrush minorBrush;
        private SolidColorBrush majorBrush;
        private SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);

        private int generativePitch;
        private int nCols;
        private int nRows;
        private int startPositionX;
        private int startPositionY;
        private int occluderAlpha;

        private int verticalSpacer;
        private int horizontalSpacer;
        private int buttonHeight;
        private int buttonWidth;
        private int occluderOffset;

        #endregion

        #region Surface components
        private Canvas canvas;

        private CalibrateButton[,] CalibrateButtons;
        #endregion

        public CalibrationSurface(Canvas canvas, IDimension dimensions, IColorCode colorCode, IButtonsSettings buttonsSettings)
        {
            LoadSettings(dimensions, colorCode, buttonsSettings);
            CalibrateButtons = new CalibrateButton[nRows, nCols];

            this.canvas = canvas;

            canvas.Width = startPositionX * 2 + (horizontalSpacer + 13) * (nCols - 1);
            canvas.Height = startPositionY * 2 + (verticalSpacer + 13) * (nRows - 1);


            canvas.Children.Add(highlighter);
        }

        private void LoadSettings(IDimension dimensions, IColorCode colorCode, IButtonsSettings buttonsSettings)
        {
            buttonHeight = dimensions.ButtonHeight;
            buttonWidth = dimensions.ButtonWidth;
            horizontalSpacer = dimensions.HorizontalSpacer;
            occluderOffset = dimensions.OccluderOffset;
            verticalSpacer = dimensions.VerticalSpacer;

            keysColorCode = colorCode.KeysColorCode;

            notInScaleBrush = colorCode.NotInScaleBrush;
            majorBrush = colorCode.MajorBrush;
            minorBrush = colorCode.MinorBrush;

            generativePitch = buttonsSettings.GenerativeNote;
            nCols = buttonsSettings.NCols;
            nRows = buttonsSettings.NRows;
            startPositionX = buttonsSettings.StartPositionX;
            startPositionY = buttonsSettings.StartPositionY;
            occluderAlpha = buttonsSettings.OccluderAlpha;

            highlighter.Width = dimensions.HighlightRadius;
            highlighter.Height = dimensions.HighlightRadius;
            highlighter.StrokeThickness = dimensions.HighlightStrokeDim;
            highlighter.Stroke = colorCode.HighlightBrush;
        }

        public void DrawButtons()
        {
            int spacer = horizontalSpacer;
            int firstSpacer = 0;
            nRows = 1;

            CalibrateButtons[0,0] = new CalibrateButton(this);

            #region Draw the button on canvas              

            int X = startPositionX + firstSpacer;
            int Y = startPositionY + verticalSpacer;
            Canvas.SetLeft(CalibrateButtons[0,0], X);
            Canvas.SetTop(CalibrateButtons[0,0], Y);

            // OCCLUDER
            CalibrateButtons[0,0].Occluder.Width = buttonWidth + occluderOffset * 2;
            CalibrateButtons[0,0].Occluder.Height = buttonHeight + occluderOffset * 2;
            CalibrateButtons[0,0].Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0xFF));

            //CalibrateButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
            CalibrateButtons[0,0].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

            Canvas.SetLeft(CalibrateButtons[0,0].Occluder, X - occluderOffset);
            Canvas.SetTop(CalibrateButtons[0,0].Occluder, Y - occluderOffset);

            Panel.SetZIndex(CalibrateButtons[0,0], 30);
            Panel.SetZIndex(CalibrateButtons[0,0].Occluder, 2);
            Panel.SetZIndex(highlighter, 30);
            canvas.Children.Add(CalibrateButtons[0,0]);
            canvas.Children.Add(CalibrateButtons[0,0].Occluder);

            CalibrateButtons[0,0].Width = buttonWidth;
            CalibrateButtons[0,0].Height = buttonHeight;
            #endregion
        }         
    }
}
