using Eyerpheus.Controllers.Graphics;
using NeeqDMIs.Music;
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
    public enum NetytarSurfaceDrawModes
    {
        AllLines,
        OnlyScaleLines,
        NoLines
    }

    public enum NetytarSurfaceHighlightModes
    {
        CurrentNote,
        None
    }

    public class NetytarSurface
    {
        private NetytarButton lastCheckedButton;
        private NetytarButton checkedButton;
        private Ellipse highlighter = new Ellipse();

        private NetytarSurfaceDrawModes drawMode;
        public NetytarSurfaceDrawModes DrawMode { get => drawMode; set => drawMode = value; }

        private Scale scale = ScalesFactory.Cmaj;
        public Scale Scale
        {
            get { return scale; }
            set { scale = value; DrawScale(); }
        }

        public NetytarButton CheckedButton { get => checkedButton; }
        public NetytarSurfaceHighlightModes HighLightMode { get; set; }

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
        private int ellipseStrokeDim;
        private int ellipseStrokeSpacer;
        private int lineThickness;
        #endregion

        #region Surface components
        private Canvas canvas;

        private NetytarButton[,] netytarButtons;
        private List<Line> drawnLines = new List<Line>();
        private List<Ellipse> drawnEllipses = new List<Ellipse>();
        #endregion

        public NetytarSurface(Canvas canvas, IDimension dimensions, IColorCode colorCode, IButtonsSettings buttonsSettings, NetytarSurfaceDrawModes drawMode)
        {
            LoadSettings(dimensions, colorCode, buttonsSettings);
            this.drawMode = drawMode;

            netytarButtons = new NetytarButton[nRows, nCols];

            this.canvas = canvas;
            /*
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.Margin = new Thickness(0, 0, 0, 0);*/
            canvas.Width = startPositionX * 2 + (horizontalSpacer + 13) * (nCols - 1);
            canvas.Height = startPositionY * 2 + (verticalSpacer + 13) * (nRows - 1);

            canvas.Children.Add(highlighter);
        }

        private void LoadSettings(IDimension dimensions, IColorCode colorCode, IButtonsSettings buttonsSettings)
        {
            buttonHeight = dimensions.ButtonHeight;
            buttonWidth = dimensions.ButtonWidth;
            ellipseStrokeDim = dimensions.EllipseStrokeDim;
            ellipseStrokeSpacer = dimensions.EllipseStrokeSpacer;
            horizontalSpacer = dimensions.HorizontalSpacer;
            lineThickness = dimensions.LineThickness;
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

        public void DrawScale()
        {
            DrawLines(scale);
            DrawEllipses(scale);
        }

        public void DrawButtons()
        {
            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?
                    if (row % 2 != 0)
                    {
                        isPairRow = false;
                    }
                    else
                    {
                        isPairRow = true;
                    }
                    #endregion

                    #region Draw the button on canvas
                    if (isPairRow)
                    {
                        firstSpacer = horizontalSpacer / 2;
                    }
                    else
                    {
                        firstSpacer = 0;
                    }

                    netytarButtons[row, col] = new NetytarButton(this);
                    int X = startPositionX + firstSpacer + col * horizontalSpacer;
                    int Y = startPositionY + verticalSpacer * row;
                    Canvas.SetLeft(netytarButtons[row, col], X);
                    Canvas.SetTop(netytarButtons[row, col], Y);

                    // OCCLUDER
                    netytarButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netytarButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    netytarButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0xFF, 0xFF, 0xFF));
                    netytarButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));
                    
                    Canvas.SetLeft(netytarButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netytarButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netytarButtons[row, col], 3);
                    Panel.SetZIndex(netytarButtons[row, col].Occluder, 2000);
                    canvas.Children.Add(netytarButtons[row, col]);
                    canvas.Children.Add(netytarButtons[row, col].Occluder);

                    netytarButtons[row, col].Width = buttonWidth;
                    netytarButtons[row, col].Height = buttonHeight;
                    #endregion

                    #region Define note
                    int calcPitch = generativePitch;
                    calcPitch += col * 2 + row * 2;
                    if (isPairRow)
                    {
                        calcPitch += 1;
                    }
                    netytarButtons[row, col].Note = PitchUtils.PitchToMidiNote(calcPitch);
                    #endregion
                }
            }
        }

        private void DrawLines(Scale scale)
        {
            if (netytarButtons != null)
            {
                ClearLines();
                if (drawMode != NetytarSurfaceDrawModes.NoLines)
                {
                    Brush inScaleBrush = new SolidColorBrush();
                    #region Determine inScale brush
                    switch (scale.ScaleCode)
                    {
                        case ScaleCodes.maj:
                            inScaleBrush = majorBrush;
                            break;
                        case ScaleCodes.min:
                            inScaleBrush = minorBrush;
                            break;
                        default:
                            inScaleBrush = majorBrush;
                            break;
                    }
                    #endregion

                    bool isPairRow;

                    Point realCenter1;
                    Point realCenter2;

                    for (int row = 0; row < nRows; row++)
                    {
                        for (int col = 0; col < nCols; col++)
                        {

                            #region Is row pair?
                            if (row % 2 != 0)
                            {
                                isPairRow = false;
                            }
                            else
                            {
                                isPairRow = true;
                            }
                            #endregion

                            #region Draw horizontal lines
                            if (col != 0)
                            {
                                Brush brush;
                                if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row, col - 1].Note))
                                {
                                    brush = inScaleBrush;
                                }
                                else
                                {
                                    if (drawMode == NetytarSurfaceDrawModes.OnlyScaleLines)
                                    {
                                        brush = transparentBrush;
                                    }
                                    else
                                    {
                                        brush = notInScaleBrush;
                                    }
                                }
                                realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 0, Canvas.GetTop(netytarButtons[row, col]) + 6);
                                realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row, col - 1]) + 13, Canvas.GetTop(netytarButtons[row, col - 1]) + 6);
                                Line myLine = new Line();


                                myLine.Stroke = brush;
                                myLine.X1 = realCenter1.X;
                                myLine.X2 = realCenter2.X;
                                myLine.Y1 = realCenter1.Y;
                                myLine.Y2 = realCenter2.Y;
                                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                                myLine.VerticalAlignment = VerticalAlignment.Center;
                                myLine.StrokeThickness = lineThickness;
                                Panel.SetZIndex(myLine, 1);
                                drawnLines.Add(myLine);

                                netytarButtons[row, col - 1].L_p2 = myLine;
                                netytarButtons[row, col].L_m2 = myLine;

                            }
                            #endregion

                            #region Draw diagonal lines

                            // Diagonale A: se riga pari p+1, se dispari p+3
                            if (row != 0)
                            {
                                Brush brush;
                                if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row - 1, col].Note))
                                {
                                    brush = inScaleBrush;
                                }
                                else
                                {
                                    if (drawMode == NetytarSurfaceDrawModes.OnlyScaleLines)
                                    {
                                        brush = transparentBrush; 
                                    }
                                    else
                                    {
                                        brush = notInScaleBrush;
                                    }
                                }

                                if (isPairRow)
                                {
                                    realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 2, Canvas.GetTop(netytarButtons[row, col]) + 2);
                                    realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col]) + 11, Canvas.GetTop(netytarButtons[row - 1, col]) + 11);
                                }
                                else
                                {
                                    realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 11, Canvas.GetTop(netytarButtons[row, col]) + 2);
                                    realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col]) + 2, Canvas.GetTop(netytarButtons[row - 1, col]) + 11);
                                }


                                Line myLine = new Line();

                                myLine.Stroke = brush;
                                myLine.X1 = realCenter1.X;
                                myLine.X2 = realCenter2.X;
                                myLine.Y1 = realCenter1.Y;
                                myLine.Y2 = realCenter2.Y;
                                myLine.HorizontalAlignment = HorizontalAlignment.Left;
                                myLine.VerticalAlignment = VerticalAlignment.Center;
                                myLine.StrokeThickness = lineThickness;
                                Panel.SetZIndex(myLine, 1);
                                drawnLines.Add(myLine);

                                if (isPairRow)
                                {
                                    netytarButtons[row - 1, col].L_p3 = myLine;
                                    netytarButtons[row, col].L_m3 = myLine;
                                }
                                else
                                {
                                    netytarButtons[row - 1, col].L_p1 = myLine;
                                    netytarButtons[row, col].L_m1 = myLine;
                                }

                                // Diagonale B: se riga pari p+3, se dispari p+1

                                if (isPairRow)
                                {

                                    if (col < nCols - 1)
                                    {
                                        if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row - 1, col + 1].Note))
                                        {
                                            brush = inScaleBrush;
                                        }
                                        else
                                        {
                                            if (drawMode == NetytarSurfaceDrawModes.OnlyScaleLines)
                                            {
                                                brush = transparentBrush;
                                            }
                                            else
                                            {
                                                brush = notInScaleBrush;
                                            }
                                        }
                                        realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 11, Canvas.GetTop(netytarButtons[row, col]) + 2);
                                        realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col + 1]) + 2, Canvas.GetTop(netytarButtons[row - 1, col + 1]) + 11);

                                        Line diaLine = new Line();


                                        diaLine.Stroke = brush;
                                        diaLine.X1 = realCenter1.X;
                                        diaLine.X2 = realCenter2.X;
                                        diaLine.Y1 = realCenter1.Y;
                                        diaLine.Y2 = realCenter2.Y;
                                        diaLine.HorizontalAlignment = HorizontalAlignment.Left;
                                        diaLine.VerticalAlignment = VerticalAlignment.Center;
                                        diaLine.StrokeThickness = lineThickness;
                                        Panel.SetZIndex(diaLine, 1);
                                        drawnLines.Add(diaLine);

                                        netytarButtons[row - 1, col + 1].L_p1 = diaLine;
                                        netytarButtons[row, col].L_m1 = diaLine;
                                    }
                                }
                                else
                                {
                                    if (col > 0)
                                    {
                                        if (scale.AreConsequent(netytarButtons[row, col].Note, netytarButtons[row - 1, col - 1].Note))
                                        {
                                            brush = inScaleBrush;

                                        }
                                        else
                                        {
                                            if (drawMode == NetytarSurfaceDrawModes.OnlyScaleLines)
                                            {
                                                brush = transparentBrush;
                                            }
                                            else
                                            {
                                                brush = notInScaleBrush;
                                            }
                                        }
                                        realCenter1 = new Point(Canvas.GetLeft(netytarButtons[row, col]) + 2, Canvas.GetTop(netytarButtons[row, col]) + 2);
                                        realCenter2 = new Point(Canvas.GetLeft(netytarButtons[row - 1, col - 1]) + 11, Canvas.GetTop(netytarButtons[row - 1, col - 1]) + 11);

                                        Line diaLine = new Line();

                                        diaLine.Stroke = brush;
                                        diaLine.X1 = realCenter1.X;
                                        diaLine.X2 = realCenter2.X;
                                        diaLine.Y1 = realCenter1.Y;
                                        diaLine.Y2 = realCenter2.Y;
                                        diaLine.HorizontalAlignment = HorizontalAlignment.Left;
                                        diaLine.VerticalAlignment = VerticalAlignment.Center;
                                        diaLine.StrokeThickness = lineThickness;
                                        Panel.SetZIndex(diaLine, 1);
                                        drawnLines.Add(diaLine);

                                        netytarButtons[row - 1, col - 1].L_p3 = diaLine;
                                        netytarButtons[row, col].L_m3 = diaLine;
                                    }
                                }

                            }
                            #endregion

                        }
                    }

                    foreach (Line line in drawnLines)
                    {
                            canvas.Children.Add(line);
                    }
                }

            
            }
        }

        public void ClearLines()
        {
            for (int i = 0; i < drawnLines.Count; i++)
            {
                Line line = drawnLines[i];
                canvas.Children.Remove(line);
            }

            drawnLines = new List<Line>();
        }
        
        public void ClearEllipses()
        {
            for (int i = 0; i < drawnEllipses.Count; i++)
            {
                Ellipse ellipse = drawnEllipses[i];
                canvas.Children.Remove(ellipse);
            }

            drawnEllipses = new List<Ellipse>();
        }
    
        public void DrawEllipses(Scale scale)
        {
            if(netytarButtons != null)
            {
                ClearEllipses();

                List<AbsNotes> scaleNotes = scale.NotesInScale;

                for (int j = 0; j < nCols; j++)
                {
                    for (int k = 0; k < nCols; k++)
                    {
                        for (int i = 0; i < scaleNotes.Count; i++)
                        {
                            AbsNotes note = netytarButtons[j, k].Note.ToAbsNote();
                            if (note == scaleNotes[i])
                            {
                                Ellipse ellipse = new Ellipse();
                                ellipse.StrokeThickness = ellipseStrokeDim;
                                ellipse.Stroke = new SolidColorBrush(keysColorCode[i]);
                                ellipse.Width = netytarButtons[j, k].Width + ellipseStrokeSpacer * 2;
                                ellipse.Height = netytarButtons[j, k].Height + ellipseStrokeSpacer * 2;
                                Canvas.SetLeft(ellipse, Canvas.GetLeft(netytarButtons[j, k]) - ellipseStrokeSpacer + 0.4);
                                Canvas.SetTop(ellipse, Canvas.GetTop(netytarButtons[j, k]) - ellipseStrokeSpacer + 0.2);
                                Canvas.SetZIndex(ellipse, 2);
                                drawnEllipses.Add(ellipse);
                            }
                        }
                    }
                }

                foreach (Ellipse ellipse in drawnEllipses)
                {
                        canvas.Children.Add(ellipse);
                }
            }
        }

        public void NetytarButton_OccluderMouseEnter(NetytarButton sender)
        {
            if(sender != CheckedButton)
            {
                Rack.NetytarDMIBox.SelectedNote = sender.Note;

                lastCheckedButton = checkedButton;
                checkedButton = sender;

                FlashMovementLine();
                // FlashSpark();

                if(HighLightMode == NetytarSurfaceHighlightModes.CurrentNote)
                {
                    MoveHighlighter(CheckedButton);
                }
            }
        }

        private void MoveHighlighter(NetytarButton checkedButton)
        {
            Canvas.SetLeft(highlighter, Canvas.GetLeft(checkedButton) - highlighter.ActualWidth / 2.5) ;
            Canvas.SetTop(highlighter, Canvas.GetTop(checkedButton) - highlighter.ActualHeight / 2.5);
        }

        public void FlashMovementLine()
        {
            if(lastCheckedButton != null)
            {
                Point point1 = new Point(Canvas.GetLeft(CheckedButton) + 6, Canvas.GetTop(CheckedButton) + 6);
                Point point2 = new Point(Canvas.GetLeft(lastCheckedButton) + 6, Canvas.GetTop(lastCheckedButton) + 6);
                IndependentLineFlashTimer timer = new IndependentLineFlashTimer(point1, point2, canvas, Colors.Black);
            }
            
        }

        public void FlashSpark()
        {
            if (checkedButton != null)
            {
                Image image = new Image();
                image.Height = 40;
                image.Width = 40;
                Canvas.SetLeft(image, Canvas.GetLeft(checkedButton) - 10);
                Canvas.SetTop(image, Canvas.GetTop(checkedButton) - 15);
                Canvas.SetZIndex(image, 100);
                canvas.Children.Add(image);

                BitmapImage bitImage = new BitmapImage();
                bitImage.BeginInit();
                bitImage.UriSource = new Uri("pack://application:,,,/Images/Sparks/Spark3.gif");
                bitImage.EndInit();
                ImageBehavior.SetAnimatedSource(image, bitImage);
                ImageBehavior.SetRepeatBehavior(image, new RepeatBehavior(1));
                ImageBehavior.AddAnimationCompletedHandler(image, disposeImage);
            }
            
        }

        private void disposeImage(object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(((Image)sender));
        }
    }
}
