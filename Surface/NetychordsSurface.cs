using Eyerpheus.Controllers.Graphics;
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
    public enum NetychordsSurfaceDrawModes
    {
        AllLines,
        OnlyScaleLines,
        NoLines
    }

    public enum NetychordsSurfaceHighlightModes
    {
        CurrentNote,
        None
    }

    public class NetychordsSurface
    {
        private NetychordsButton lastCheckedButton;
        private NetychordsButton checkedButton;
        public Ellipse highlighter = new Ellipse(); //before was private

        private NetychordsSurfaceDrawModes drawMode;
        public NetychordsSurfaceDrawModes DrawMode { get => drawMode; set => drawMode = value; }
        public NetychordsButton CheckedButton { get => checkedButton; }
        public NetychordsSurfaceHighlightModes HighLightMode { get; set; }

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
        public int nCols;
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

        private string starterOctave;
        private string starterNote;
        public MidiChord firstChord; //before was private
        private MidiChord actualChord;

        #endregion

        #region Surface components
        private Canvas canvas;

        private NetychordsButton[,] NetychordsButtons;
        private List<Line> drawnLines = new List<Line>();
        private List<Ellipse> drawnEllipses = new List<Ellipse>();
        #endregion

        public NetychordsSurface(Canvas canvas, IDimension dimensions, IColorCode colorCode, IButtonsSettings buttonsSettings, NetychordsSurfaceDrawModes drawMode)
        {
            LoadSettings(dimensions, colorCode, buttonsSettings);

            nRows = 11; // System.Enum.GetNames(typeof(ChordType)).Length;

            this.drawMode = drawMode;

            NetychordsButtons = new NetychordsButton[nRows, nCols];

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

        public void DrawButtons()
        {
            if (Rack.NetychordsDMIBox.MainWindow.lstNoteChanger.SelectedItem != null)
            {
                starterNote = ((ListBoxItem)Rack.NetychordsDMIBox.MainWindow.lstNoteChanger.SelectedItem).Content.ToString();
            }
            else
            {
                starterNote = "C";
            }

            if (true)//Rack.NetychordsDMIBox.MainWindow.lstOctaveChanger.SelectedItem != null)
            {
                //starterOctave = ((ListBoxItem)Rack.NetychordsDMIBox.MainWindow.lstOctaveChanger.SelectedItem).Content.ToString();
               // nCols = 96 - 12 * Int32.Parse(starterOctave);
                nCols = 12;

            }
            else
            {
                //nCols = 96 - 12 * 4;
                nCols = 12;
                //starterOctave = Rack.NetychordsDMIBox.octaveNumber;
            }

            /*if ((bool)Rack.NetychordsDMIBox.MainWindow.one.IsChecked)
            {
                starterOctave = "1";
            }
            else if ((bool)Rack.NetychordsDMIBox.MainWindow.two.IsChecked)
            {
                starterOctave = "2";
            }
            else if ((bool)Rack.NetychordsDMIBox.MainWindow.three.IsChecked)
            {
                starterOctave = "3";
            }
            else if ((bool)Rack.NetychordsDMIBox.MainWindow.four.IsChecked)
            {
                starterOctave = "4";
            }
            else if ((bool)Rack.NetychordsDMIBox.MainWindow.five.IsChecked)
            {
                starterOctave = "5";
            }
            else if ((bool)Rack.NetychordsDMIBox.MainWindow.six.IsChecked)
            {
                starterOctave = "6";
            }
            else if ((bool)Rack.NetychordsDMIBox.MainWindow.seven.IsChecked)
            {
                starterOctave = "7";
            }
            else
            {
                starterOctave = "4";
            }*/

            firstChord = MidiChord.ChordFactory(starterNote, "2", ChordType.Major);
            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;


            if (Rack.NetychordsDMIBox.arbitraryLines.Count != 0)
            {
                nRows = Rack.NetychordsDMIBox.arbitraryLines.Count;
            }
            else
            {
                if (Rack.NetychordsDMIBox.layout == "Stradella" || Rack.NetychordsDMIBox.layout == "Fifth circle")
                {
                    nRows = 11;
                }
                else if (Rack.NetychordsDMIBox.layout == "Jazz")
                {
                    nRows = 6;
                }
                else if (Rack.NetychordsDMIBox.layout == "Pop" || Rack.NetychordsDMIBox.layout == "Rock")
                {
                    nRows = 4;
                }
                else
                {
                    nRows = 11;
                }
            }

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?
                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        if (Rack.NetychordsDMIBox.layout == "Stradella")
                        {

                            spacer = 100;
                            firstSpacer = row * spacer / 4;
                        }
                        else
                        {
                            spacer = horizontalSpacer;
                            firstSpacer = row * spacer / 2;
                        }
                        

                        if (row % 2 != 0)
                        {
                            isPairRow = false;
                        }
                        else
                        {
                            isPairRow = true;
                        }
                        verticalSpacer = -70;
                        canvas.Height = startPositionY * 2 + (verticalSpacer + 13) * (nRows - 1);

                    }
                    else
                    {
                        spacer = 90;
                        firstSpacer = 0;
                        isPairRow = true;
                        verticalSpacer = 90;
                        canvas.Height = startPositionY * 2 + (verticalSpacer + 13) * (nRows - 1);
                    }

                    #endregion

                    NetychordsButtons[row, col] = new NetychordsButton(this);

                    #region Define chordType of this chord and starter note of the row
                    ChordType thisChordType;
                    MidiNotes thisNote;

                    if (Rack.NetychordsDMIBox.layout == "Arbitrary" && Rack.NetychordsDMIBox.arbitraryLines.Count != 0)
                    {                        
                            string type = Rack.NetychordsDMIBox.arbitraryLines[Rack.NetychordsDMIBox.arbitraryLines.Count - row - 1];
                        
                            switch (type)
                            {
                                case "Sus2":
                                    thisChordType = ChordType.Sus2;
                                    firstSpacer = 0;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "Sus4":
                                    thisChordType = ChordType.Sus4;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "DiminishedSeventh":
                                    thisChordType = ChordType.DiminishedSeventh;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "Major":
                                    thisChordType = ChordType.Major;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "DominantSeventh":
                                    thisChordType = ChordType.DominantSeventh;
                                    if (firstChord.chordType != ChordType.DominantSeventh)
                                    {
                                        actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                        firstChord.chordType = ChordType.DominantSeventh;

                                    };

                                    if (col == 0)
                                    {
                                        thisNote = actualChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "Minor":
                                    thisChordType = ChordType.Minor;
                                    if (firstChord.chordType != ChordType.Minor)
                                    {
                                        actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                        firstChord.chordType = ChordType.Minor;
                                    };

                                    if (col == 0)
                                    {
                                        thisNote = actualChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "DominantNinth":
                                    thisChordType = ChordType.DominantNinth;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "MajorSeventh":
                                    thisChordType = ChordType.MajorSeventh;
                                    if (firstChord.chordType != ChordType.MajorSeventh)
                                    {
                                        actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                        firstChord.chordType = ChordType.MajorSeventh;

                                    };

                                    if (col == 0)
                                    {
                                        thisNote = actualChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "DominantEleventh":
                                    thisChordType = ChordType.DominantEleventh;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "MinorSeventh":
                                    thisChordType = ChordType.MinorSeventh;
                                    if (firstChord.chordType != ChordType.MinorSeventh)
                                    {
                                        actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                        firstChord.chordType = ChordType.MinorSeventh;

                                    };

                                    if (col == 0)
                                    {
                                        thisNote = actualChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                case "Augmented":
                                    thisChordType = ChordType.Augmented;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                                default:
                                    thisChordType = ChordType.Major;
                                    if (col == 0)
                                    {
                                        thisNote = firstChord.rootNote;
                                    }
                                    else if (col % 2 != 0)
                                    {
                                        thisNote = actualChord.rootNote - 5;
                                    }
                                    else
                                    {
                                        thisNote = actualChord.rootNote + 7;
                                    };
                                    actualChord = new MidiChord(thisNote, thisChordType);
                                    NetychordsButtons[row, col].Chord = actualChord;
                                    break;
                            }
                        
                    }
                    else if (Rack.NetychordsDMIBox.layout == "Pop" || Rack.NetychordsDMIBox.layout == "Rock")
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.Sus2;
                                firstSpacer = 0;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 1:
                                thisChordType = ChordType.Sus4;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 2:
                                thisChordType = ChordType.Major;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 3:
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Rack.NetychordsDMIBox.layout == "Jazz")
                    {
                        switch (row)
                        {
                            
                            case 0:
                                thisChordType = ChordType.DiminishedSeventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 1:
                                thisChordType = ChordType.Major;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 2:
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;

                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 3:
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 4:
                                thisChordType = ChordType.MajorSeventh;
                                if (firstChord.chordType != ChordType.MajorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                    firstChord.chordType = ChordType.MajorSeventh;

                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 5:
                                thisChordType = ChordType.MinorSeventh;
                                if (firstChord.chordType != ChordType.MinorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                    firstChord.chordType = ChordType.MinorSeventh;

                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Rack.NetychordsDMIBox.layout != "Stradella")
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.Sus2;
                                firstSpacer = 0;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 1:
                                thisChordType = ChordType.Sus4;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 2:
                                thisChordType = ChordType.DiminishedSeventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 3:
                                thisChordType = ChordType.Major;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 4:
                                thisChordType = ChordType.DominantSeventh;
                                if (firstChord.chordType != ChordType.DominantSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.DominantSeventh);
                                    firstChord.chordType = ChordType.DominantSeventh;

                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 5:
                                thisChordType = ChordType.Minor;
                                if (firstChord.chordType != ChordType.Minor)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                    firstChord.chordType = ChordType.Minor;
                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 6:
                                thisChordType = ChordType.DominantNinth;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 7:
                                thisChordType = ChordType.MajorSeventh;
                                if (firstChord.chordType != ChordType.MajorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                    firstChord.chordType = ChordType.MajorSeventh;

                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 8:
                                thisChordType = ChordType.DominantEleventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 9:
                                thisChordType = ChordType.MinorSeventh;
                                if (firstChord.chordType != ChordType.MinorSeventh)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote + 2, ChordType.MinorSeventh);
                                    firstChord.chordType = ChordType.MinorSeventh;

                                };

                                if (col == 0)
                                {
                                    thisNote = actualChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 10:
                                thisChordType = ChordType.Augmented;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.Sus2;
                                firstSpacer = 0;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 1:
                                thisChordType = ChordType.Sus4;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 2:
                                thisChordType = ChordType.DiminishedSeventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote + 7;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote - 5;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 3:
                                thisChordType = ChordType.Major;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 4:
                                thisChordType = ChordType.DominantSeventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 5:
                                thisChordType = ChordType.Minor;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 6:
                                thisChordType = ChordType.DominantNinth;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 7:
                                thisChordType = ChordType.MajorSeventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 8:
                                thisChordType = ChordType.DominantEleventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 9:
                                thisChordType = ChordType.MinorSeventh;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            case 10:
                                thisChordType = ChordType.Augmented;
                                if (col == 0)
                                {
                                    thisNote = firstChord.rootNote;
                                }
                                else if (col % 2 != 0)
                                {
                                    thisNote = actualChord.rootNote - 5;
                                }
                                else
                                {
                                    thisNote = actualChord.rootNote + 7;
                                };
                                actualChord = new MidiChord(thisNote, thisChordType);
                                NetychordsButtons[row, col].Chord = actualChord;
                                break;
                            default:
                                break;
                        }
                    }

                    #endregion

                    #region Draw the button on canvas
                    if (Rack.NetychordsDMIBox.layout != "Stradella")
                    {
                        if (!isPairRow)
                        {
                            firstSpacer = spacer / 2;
                        }
                        else
                        {
                            firstSpacer = 0;
                        }
                    }                    

                    int X = startPositionX + firstSpacer + col * spacer;
                    int Y;
                    if (Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        Y = startPositionY + verticalSpacer * row;
                    }
                    else
                    {
                        Y = startPositionY - verticalSpacer * row;

                    }
                    Canvas.SetLeft(NetychordsButtons[row, col], X);
                    Canvas.SetTop(NetychordsButtons[row, col], Y);

                    // OCCLUDER
                    NetychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    NetychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    NetychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(NetychordsButtons[row, col]);

                    Canvas.SetLeft(NetychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(NetychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(NetychordsButtons[row, col], 30);
                    Panel.SetZIndex(NetychordsButtons[row, col].Occluder, 2);
                    Panel.SetZIndex(highlighter, 30);
                    canvas.Children.Add(NetychordsButtons[row, col]);
                    canvas.Children.Add(NetychordsButtons[row, col].Occluder);

                    NetychordsButtons[row, col].Width = buttonWidth;
                    NetychordsButtons[row, col].Height = buttonHeight;
                    #endregion

                    /*
                    #region Define rootNote of this chord

                    int calcShift;
                    if (col % 2 != 0)
                    {
                        calcShift = col * 7;
                    }
                    else
                    {
                        calcShift = col * 7;
                    }
                    
                    MidiNotes thisNote = firstChord.rootNote + calcShift;

                    #endregion



                    NetychordsButtons[row, col].Chord = new MidiChord(thisNote, thisChordType);*/
                }
            }
        }
            
        public void NetychordsButton_OccluderMouseEnter(NetychordsButton sender)
        {
            if(sender != CheckedButton)
            {
                Rack.NetychordsDMIBox.lastChord = Rack.NetychordsDMIBox.Chord;
                Rack.NetychordsDMIBox.Chord = sender.Chord;

                lastCheckedButton = checkedButton;
                checkedButton = sender;

                FlashMovementLine();

                if(HighLightMode == NetychordsSurfaceHighlightModes.CurrentNote)
                {
                    MoveHighlighter(CheckedButton);
                }
            }
        }

        private void MoveHighlighter(NetychordsButton checkedButton)
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
                IndependentLineFlashTimer timer = new IndependentLineFlashTimer(point1, point2, canvas, Colors.NavajoWhite);
            }
            
        }

        private void DisposeImage(object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(((Image)sender));
        }


        private void NoteToColor(NetychordsButton button)
        {
            string n = actualChord.rootNote.ToStandardString();
            switch (n.Remove(n.Length - 1))
            {
                case "C":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x00, 0x00));
                    break;
                case "C#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xa9, 0x8a, 0x4d));
                    break;
                case "D":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xA5, 0x00));
                    break;
                case "D#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xD7, 0x00));//
                    break;
                case "E":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0x00));
                    break;
                case "F":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x90, 0xEE, 0x90));//
                    break;
                case "F#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0x00));
                    break;
                case "G":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0xFF, 0xFF));
                    break;
                case "G#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0x00, 0xFF));
                    break;
                case "A":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x00, 0x00, 0xFF));
                    break;
                case "A#":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xC0, 0xCB));//
                    break;
                case "B":
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0x6F, 0x00, 0xFF));
                    break;
                default:
                    button.Occluder.Fill = new SolidColorBrush(Color.FromArgb(255, 0xFF, 0xFF, 0xFF));
                    break;
            }
            
        }
    }
}
