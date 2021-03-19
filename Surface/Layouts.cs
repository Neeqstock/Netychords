using NeeqDMIs.Music;
using Netychords.Surface.FlowerLayout;
using Netychords.Utils;
using System.Windows.Controls;
using System.Windows.Media;

namespace Netychords.Surface
{
    public enum Layouts
    {
        FifthCircle,
        Arbitrary,
        Stradella,
        Jazz,
        Pop,
        Rock,
        Flower
    }

    public static class LayoutsMethods
    {
        private static int buttonHeight;
        private static int buttonWidth;
        private static int horizontalSpacer;
        private static int nCols;
        private static int nRows;
        private static int occluderAlpha;
        private static int occluderOffset;
        private static int startPositionX;
        private static int startPositionY;
        private static int verticalSpacer;

        public static void Draw(this Layouts layout, MidiChord firstChord, Canvas canvas, NetychordsButton[,] NetychordsButtons)
        {
            canvas.Children.Clear();

            switch (layout)
            {
                case Layouts.FifthCircle:
                    DrawFifthCircle(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Arbitrary:
                    DrawArbitrary(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Stradella:
                    DrawStradella(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Jazz:
                    DrawJazz(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Pop:
                    DrawPop(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Rock:
                    DrawRock(firstChord, canvas, NetychordsButtons);
                    break;

                case Layouts.Flower:
                    DrawFlower(firstChord, canvas, NetychordsButtons);
                    break;
            }
        }

        /// <summary>
        /// Deprecated!
        /// </summary>
        /// <param name="firstChord">        </param>
        /// <param name="canvas">            </param>
        /// <param name="netychordsButtons"> </param>
        private static void Draw_Old(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            if (Rack.NetychordsDMIBox.arbitraryLines.Count != 0)
            {
                nRows = Rack.NetychordsDMIBox.arbitraryLines.Count;
            }
            else
            {
                if (Rack.NetychordsDMIBox.Layout == Layouts.Stradella || Rack.NetychordsDMIBox.Layout == Layouts.FifthCircle)
                {
                    nRows = 11;
                }
                else if (Rack.NetychordsDMIBox.Layout == Layouts.Jazz)
                {
                    nRows = 7;
                }
                else if (Rack.NetychordsDMIBox.Layout == Layouts.Pop)
                {
                    nRows = 4;
                }
                else if (Rack.NetychordsDMIBox.Layout == Layouts.Rock)
                {
                    nRows = 5;
                }
                else
                {
                    nRows = 11;
                }
            }

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        if (Rack.NetychordsDMIBox.Layout == Layouts.Stradella)
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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    if (Rack.NetychordsDMIBox.Layout == Layouts.Arbitrary && Rack.NetychordsDMIBox.arbitraryLines.Count != 0)
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case "MajorSixth":
                                thisChordType = ChordType.MajorSixth;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case "MinorSixth":
                                thisChordType = ChordType.MinorSixth;
                                if (firstChord.chordType != ChordType.MinorSixth)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                    firstChord.chordType = ChordType.MinorSixth;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case "SemiDiminished":
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;
                        }
                    }
                    else if (Rack.NetychordsDMIBox.Layout == Layouts.Pop)
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            default:
                                break;
                        }
                    }
                    else if (Rack.NetychordsDMIBox.Layout == Layouts.Rock)
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            default:
                                break;
                        }
                    }
                    else if (Rack.NetychordsDMIBox.Layout == Layouts.Jazz)
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 1:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 2:
                                thisChordType = ChordType.MajorSixth;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 3:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 4:
                                thisChordType = ChordType.MinorSixth;
                                if (firstChord.chordType != ChordType.MinorSixth)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                    firstChord.chordType = ChordType.MinorSixth;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 5:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 6:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            default:
                                break;
                        }
                    }
                    else if (Rack.NetychordsDMIBox.Layout != Layouts.Stradella)
                    {
                        switch (row)
                        {
                            case 0:
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 1:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 2:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 3:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 4:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 5:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 6:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 7:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 8:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 9:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 10:
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                thisChordType = ChordType.SemiDiminished;

                                firstSpacer = 0;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 1:
                                thisChordType = ChordType.Sus2;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 2:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 3:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 4:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 5:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 6:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 7:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 8:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 9:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case 10:
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;
                            /*case 10:
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
                                break;*/
                            default:
                                break;
                        }
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (Rack.NetychordsDMIBox.Layout != Layouts.Stradella)
                    {
                        if (!isPairRow)
                        {
                            firstSpacer = 0;
                        }
                        else
                        {
                            firstSpacer = spacer / 2;
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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void DrawArbitrary(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // INIZIALIZZAZIONE NUMERO RIGHE =====================
            if (Rack.NetychordsDMIBox.arbitraryLines.Count != 0)
            {
                nRows = Rack.NetychordsDMIBox.arbitraryLines.Count;
            }

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    if (Rack.NetychordsDMIBox.arbitraryLines.Count != 0)
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case "MajorSixth":
                                thisChordType = ChordType.MajorSixth;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case "MinorSixth":
                                thisChordType = ChordType.MinorSixth;
                                if (firstChord.chordType != ChordType.MinorSixth)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                    firstChord.chordType = ChordType.MinorSixth;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;

                            case "SemiDiminished":
                                thisChordType = ChordType.SemiDiminished;
                                if (firstChord.chordType != ChordType.SemiDiminished)
                                {
                                    actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                    firstChord.chordType = ChordType.SemiDiminished;
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
                                netychordsButtons[row, col].Chord = actualChord;
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
                                netychordsButtons[row, col].Chord = actualChord;
                                break;
                        }
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void DrawFifthCircle(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 11;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.DiminishedSeventh;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote + 7;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote - 5;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.DominantEleventh;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
                            thisChordType = ChordType.DominantNinth;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 3:
                            thisChordType = ChordType.Sus2;
                            firstSpacer = 0;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 4:
                            thisChordType = ChordType.Sus4;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 5:
                            thisChordType = ChordType.DominantSeventh;
                            if (firstChord.chordType != ChordType.DominantSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 5, ChordType.DominantSeventh);
                                firstChord.chordType = ChordType.DominantSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote + 7;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote - 5;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 6:
                            thisChordType = ChordType.Major;
                            if (col == 0)
                            {
                                thisNote = firstChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 7:
                            thisChordType = ChordType.Minor;
                            if (firstChord.chordType != ChordType.Minor)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.Minor);
                                firstChord.chordType = ChordType.Minor;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote + 7;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote - 5;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 8:
                            thisChordType = ChordType.SemiDiminished;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote + 7;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote - 5;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 9:
                            thisChordType = ChordType.MajorSeventh;
                            if (firstChord.chordType != ChordType.MajorSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.MajorSeventh);
                                firstChord.chordType = ChordType.MajorSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 10:
                            thisChordType = ChordType.MinorSeventh;
                            if (firstChord.chordType != ChordType.MinorSeventh)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSeventh);
                                firstChord.chordType = ChordType.MinorSeventh;
                            };

                            if (col == 0)
                            {
                                thisNote = actualChord.rootNote;
                                actualChord = new MidiChord(thisNote, thisChordType);
                            }
                            /*else if (col % 2 != 0)
                            {
                                thisNote = actualChord.rootNote - 5;
                            }*/
                            else
                            {
                                //thisNote = actualChord.rootNote + 7;
                                actualChord = actualChord.generateNextFifth();
                            };
                            //actualChord = new MidiChord(thisNote, thisChordType);
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (!isPairRow)
                    {
                        firstSpacer = 0;
                    }
                    else
                    {
                        firstSpacer = spacer / 2;
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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void DrawFlower(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            System.Drawing.Point center = new System.Drawing.Point(10, 10);
            FlowerGridDimensions gridDim = new FlowerGridDimensions(40, 40);

            Plant plant = new Plant(firstChord.rootNote, PlantFamilies.Major, center);

            foreach (Flower flower in plant.Flowers)
            {
                foreach (FlowerButton flowerButton in flower.FlowerButtons)
                {
                    Canvas.SetLeft(flowerButton, flowerButton.Coordinates.X * gridDim.X + center.X);
                    Canvas.SetTop(flowerButton, flowerButton.Coordinates.Y * gridDim.Y + center.Y);

                    Canvas.SetLeft(flowerButton.Occluder, Canvas.GetLeft(flowerButton) - occluderOffset);
                    Canvas.SetTop(flowerButton.Occluder, Canvas.GetTop(flowerButton) - occluderOffset);

                    canvas.Children.Add(flowerButton);
                    canvas.Children.Add(flowerButton.Occluder);
                }
            }
        }

        private static void DrawJazz(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================

            nRows = 7;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.SemiDiminished;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 1, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
                            thisChordType = ChordType.MajorSixth;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 3:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 4:
                            thisChordType = ChordType.MinorSixth;
                            if (firstChord.chordType != ChordType.MinorSixth)
                            {
                                actualChord = new MidiChord(firstChord.rootNote - 3, ChordType.MinorSixth);
                                firstChord.chordType = ChordType.MinorSixth;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 5:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 6:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (!isPairRow)
                    {
                        firstSpacer = 0;
                    }
                    else
                    {
                        firstSpacer = spacer / 2;
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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void DrawPop(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 4;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

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
                            netychordsButtons[row, col].Chord = actualChord;
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
                            netychordsButtons[row, col].Chord = actualChord;
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
                            netychordsButtons[row, col].Chord = actualChord;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (!isPairRow)
                    {
                        firstSpacer = 0;
                    }
                    else
                    {
                        firstSpacer = spacer / 2;
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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void DrawRock(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================

            nRows = 5;

            // CICLO PRINCIPALE =====================

            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        spacer = horizontalSpacer;
                        firstSpacer = row * spacer / 2;

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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

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
                            netychordsButtons[row, col].Chord = actualChord;
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
                            netychordsButtons[row, col].Chord = actualChord;
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
                            netychordsButtons[row, col].Chord = actualChord;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

                    if (!isPairRow)
                    {
                        firstSpacer = 0;
                    }
                    else
                    {
                        firstSpacer = spacer / 2;
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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void DrawStradella(MidiChord firstChord, Canvas canvas, NetychordsButton[,] netychordsButtons)
        {
            LoadSettings();

            MidiChord actualChord = null;

            int halfSpacer = horizontalSpacer / 2;
            int spacer = horizontalSpacer;
            int firstSpacer = 0;

            bool isPairRow;

            // OVERRIDE NUMERO RIGHE PER LAYOUTS SPECIFICI =====================
            nRows = 11;

            // CICLO PRINCIPALE =====================
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    #region Is row pair?

                    if ((int)Rack.NetychordsDMIBox.MainWindow.Margins.Value == 1)
                    {
                        spacer = 100;
                        firstSpacer = row * spacer / 4;

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

                    #endregion Is row pair?

                    netychordsButtons[row, col] = new NetychordsButton(Rack.NetychordsDMIBox.NetychordsSurface);

                    #region Define chordType of this chord and starter note of the row

                    ChordType thisChordType;
                    MidiNotes thisNote;

                    switch (row)
                    {
                        case 0:
                            thisChordType = ChordType.SemiDiminished;

                            firstSpacer = 0;
                            if (firstChord.chordType != ChordType.SemiDiminished)
                            {
                                actualChord = new MidiChord(firstChord.rootNote, ChordType.SemiDiminished);
                                firstChord.chordType = ChordType.SemiDiminished;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 1:
                            thisChordType = ChordType.Sus2;
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 2:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 3:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 4:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 5:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 6:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 7:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 8:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 9:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;

                        case 10:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;
                        /*case 10:
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
                            netychordsButtons[row, col].Chord = actualChord;
                            break;*/
                        default:
                            break;
                    }

                    #endregion Define chordType of this chord and starter note of the row

                    #region Draw the button on canvas

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
                    Canvas.SetLeft(netychordsButtons[row, col], X);
                    Canvas.SetTop(netychordsButtons[row, col], Y);

                    // OCCLUDER
                    netychordsButtons[row, col].Occluder.Width = buttonWidth + occluderOffset * 2;
                    netychordsButtons[row, col].Occluder.Height = buttonHeight + occluderOffset * 2;
                    //NetychordsButtons[row, col].Occluder.Fill = new SolidColorBrush(Color.FromArgb(60, 0xFF, 0xFF, 0xFF)); //60 was (byte)occluderAlpha
                    netychordsButtons[row, col].Occluder.Stroke = new SolidColorBrush(Color.FromArgb((byte)occluderAlpha, 0, 0, 0));

                    //OCCLUDER COLORS
                    NoteToColor(netychordsButtons[row, col], actualChord);

                    Canvas.SetLeft(netychordsButtons[row, col].Occluder, X - occluderOffset);
                    Canvas.SetTop(netychordsButtons[row, col].Occluder, Y - occluderOffset);

                    Panel.SetZIndex(netychordsButtons[row, col], 30);
                    Panel.SetZIndex(netychordsButtons[row, col].Occluder, 2);
                    canvas.Children.Add(netychordsButtons[row, col]);
                    canvas.Children.Add(netychordsButtons[row, col].Occluder);

                    netychordsButtons[row, col].Width = buttonWidth;
                    netychordsButtons[row, col].Height = buttonHeight;

                    #endregion Draw the button on canvas
                }
            }
        }

        private static void LoadSettings()
        {
            horizontalSpacer = Rack.NetychordsDMIBox.NetychordsSurface.Dimension.HorizontalSpacer;
            verticalSpacer = Rack.NetychordsDMIBox.NetychordsSurface.Dimension.VerticalSpacer;
            startPositionY = Rack.NetychordsDMIBox.NetychordsSurface.ButtonSettings.StartPositionY;
            startPositionX = Rack.NetychordsDMIBox.NetychordsSurface.ButtonSettings.StartPositionX;
            nRows = Rack.NetychordsDMIBox.NetychordsSurface.ButtonSettings.NRows;
            nCols = Rack.NetychordsDMIBox.NetychordsSurface.ButtonSettings.NCols;
            occluderOffset = Rack.NetychordsDMIBox.NetychordsSurface.Dimension.OccluderOffset;
            buttonWidth = Rack.NetychordsDMIBox.NetychordsSurface.Dimension.ButtonWidth;
            buttonHeight = Rack.NetychordsDMIBox.NetychordsSurface.Dimension.ButtonHeight;
            occluderAlpha = Rack.NetychordsDMIBox.NetychordsSurface.ButtonSettings.OccluderAlpha;
        }

        private static void NoteToColor(NetychordsButton button, MidiChord actualChord)
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

        /*
         * Deprecated
         */
    }
}