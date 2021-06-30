using NeeqDMIs.Eyetracking.PointFilters;
using Netychords.DMIBox;
using Netychords.Surface;
using Netychords.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Netychords
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        //private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);

        private DateTime centering = new DateTime(2020, 01, 01, 0, 0, 0);
        private DateTime clicked;
        private bool clickedButton = false;
        private bool netychordsStarted = false;
        private int sensorPort = 1;
        private DispatcherTimer updater;

        public bool NetychordsStarted { get => netychordsStarted; set => netychordsStarted = value; }

        public int SensorPort
        {
            get { return sensorPort; }
            set
            {
                if (value > 0)
                {
                    sensorPort = value;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initializing dispatcher timer, i.e. the timer that updates every graphical value in
            // the interface.
            updater = new DispatcherTimer();
            updater.Interval = new TimeSpan(1000);
            updater.Tick += UpdateWindow;
            updater.Start();
        }

        private void ArbitraryStart_Click(object sender, RoutedEventArgs e)
        {
            List<ComboBox> boxes = new List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            Rack.NetychordsDMIBox.arbitraryLines.Clear();

            for (int i = 0; i < 11; i++)
            {
                if (boxes[i].SelectedItem != null)
                {
                    Rack.NetychordsDMIBox.arbitraryLines.Add(boxes[i].SelectedItem.ToString());
                }
                else
                {
                    break;
                }
            }
            if (netychordsStarted)
            {
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
                arbitraryStart.IsEnabled = false;
            }
        }

        private void BtnCenter_Click(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.HTData.CalibrateCenter();
            Rack.NetychordsDMIBox.CalibrationHeadSensor();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMIDIchMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.NetychordsDMIBox.MidiModule.OutDevice--;
                lblMIDIch.Text = "MP" + Rack.NetychordsDMIBox.MidiModule.OutDevice.ToString();
                clicked = DateTime.Now;
                clickedButton = true;
                btnMIDIchMinus.IsEnabled = false;
                CheckMidiPort();
            }
        }

        private void BtnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.NetychordsDMIBox.MidiModule.OutDevice++;
                lblMIDIch.Text = "MP" + Rack.NetychordsDMIBox.MidiModule.OutDevice.ToString();
                clicked = DateTime.Now;
                clickedButton = true;
                btnMIDIchPlus.IsEnabled = false;

                CheckMidiPort();
            }
        }

        private void BtnSensorPortMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                SensorPort--;
                UpdateSensorConnection();
                clicked = DateTime.Now;
                clickedButton = true;
                btnSensorPortMinus.IsEnabled = false;
            }
        }

        private void BtnSensorPortPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                SensorPort++;
                UpdateSensorConnection();
                clicked = DateTime.Now;
                clickedButton = true;
                btnSensorPortPlus.IsEnabled = false;
            }
        }

        private void CanvasNetytchords_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        private void centerZone_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Rack.NetychordsDMIBox.CenterZone = sldCenterZone.Value;
        }

        /// <summary>
        /// Checks if the selected MIDI port is available
        /// </summary>
        private void CheckMidiPort()
        {
            if (Rack.NetychordsDMIBox.MidiModule.IsMidiOk())
            {
                lblMIDIch.Foreground = ActiveBrush;
            }
            else
            {
                lblMIDIch.Foreground = WarningBrush;
            }
        }

        private void five_Checked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Add(4);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void five_Unchecked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Remove(4);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void four_Checked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Add(3);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void four_Unchecked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Remove(3);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void InitializeSensorPortText()
        {
            txtSensorPort.Foreground = WarningBrush;
            txtSensorPort.Text = "COM" + SensorPort;
            UpdateSensorConnection();
        }

        private void lstFeedbackModeChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBoxItem)lstFeedbackModeChanger.SelectedItem).Content.ToString())
            {
                case "Bars":
                    Rack.NetychordsDMIBox.NetychordsSurface.HtFeedbackModule.Mode = (Netychords.Surface.HTFeedbackModule.HTFeedbackModes)0;
                    break;

                case "HalfButton":
                    Rack.NetychordsDMIBox.NetychordsSurface.HtFeedbackModule.Mode = (Netychords.Surface.HTFeedbackModule.HTFeedbackModes)1;
                    break;

                case "DeadZone":
                    Rack.NetychordsDMIBox.NetychordsSurface.HtFeedbackModule.Mode = (Netychords.Surface.HTFeedbackModule.HTFeedbackModes)2;
                    break;
            }
        }

        private void LstLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBoxItem)lstLayout.SelectedItem).Content.ToString())
            {
                case "Fifth Circle":
                    Rack.NetychordsDMIBox.Layout = Layouts.FifthCircle;
                    break;

                case "Arbitrary":
                    Rack.NetychordsDMIBox.Layout = Layouts.Arbitrary;
                    break;

                case "Jazz":
                    Rack.NetychordsDMIBox.Layout = Layouts.Jazz;
                    break;

                case "Pop":
                    Rack.NetychordsDMIBox.Layout = Layouts.Pop;
                    break;

                case "Rock":
                    Rack.NetychordsDMIBox.Layout = Layouts.Rock;
                    break;

                case "Stradella":
                    Rack.NetychordsDMIBox.Layout = Layouts.Stradella;
                    break;

                case "Flower":
                    Rack.NetychordsDMIBox.Layout = Layouts.Flower;
                    break;
            }

            if (Rack.NetychordsDMIBox.Layout == Layouts.Arbitrary)
            {
                SetupComboBox();
                FirstRow.IsEnabled = true;
            }
            else
            {
                System.Collections.Generic.List<ComboBox> boxes = new System.Collections.Generic.List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
                for (int i = 0; i < 11; i++)
                {
                    boxes[i].IsEnabled = false;
                    boxes[i].SelectedItem = null;
                }
            }

            if (netychordsStarted && Rack.NetychordsDMIBox.Layout != Layouts.Arbitrary)
            {
                Rack.NetychordsDMIBox.arbitraryLines = new List<string>();

                arbitraryStart.IsEnabled = false;
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void LstNoteChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rack.NetychordsDMIBox.firstNote = ((ListBoxItem)lstNoteChanger.SelectedItem).Content.ToString();
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void LstOctaveChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Margins_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void one_Checked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Add(0);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
            lblYaw.Text = Rack.NetychordsDMIBox.reeds[0].ToString();
        }

        private void one_Unchecked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Remove(0);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void SelectorRow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ComboBox> boxes = new List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            for (int i = 0; i < 11; i++)
            {
                if (sender == boxes[i] && i != 10)
                {
                    boxes[i + 1].IsEnabled = true;
                    //Rack.DMIBox.arbitraryLines.Add(boxes[i].SelectedItem.ToString());
                    break;
                }
            }
            if (arbitraryStart.IsEnabled == false) arbitraryStart.IsEnabled = true;
        }

        private void SetupComboBox()
        {
            System.Collections.Generic.List<ComboBox> boxes = new System.Collections.Generic.List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            for (int i = 0; i < 11; i++)
            {
                boxes[i].Items.Clear();
                for (int j = 0; j < 13; j++)
                {
                    boxes[i].Items.Add(((ChordType)j).ToString());
                }
            }
        }

        /// <summary>
        /// This gets called when the Start button is pressed
        /// </summary>
        private void StartNetytar(object sender, RoutedEventArgs e)
        {
            if (!netychordsStarted)
            {
                // Launches the Setup class
                NetychordsSetup netychordsSetup = new NetychordsSetup(this);
                netychordsSetup.Setup();

                // Changes the aspect of the Start button
                btnStart.IsEnabled = false;
                btnStart.Foreground = new SolidColorBrush(Colors.Black);
                // Checks the selected MIDI port is available
                CheckMidiPort();
                InitializeSensorPortText();

                // LEAVE AT THE END! This keeps track of the started state
                netychordsStarted = true;
            }
            else
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.AutoScroller = new AutoScroller(Rack.NetychordsDMIBox.MainWindow.scrlNetychords, 0, 100, new PointFilterMAExpDecaying(0.1f));
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                IDimension dimension = new DimensionInvert();
                IColorCode colorCode = new ColorCodeStandard();
                IButtonsSettings buttonsSettings = new ButtonsSettingsChords();

                NetychordsSurfaceDrawModes drawMode = NetychordsSurfaceDrawModes.NoLines;
                Rack.NetychordsDMIBox.NetychordsSurface = new NetychordsSurface(Rack.NetychordsDMIBox.MainWindow.canvasNetychords, dimension, colorCode, buttonsSettings, drawMode);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                //canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
                btnStart.IsEnabled = false;
                btnStart.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void TabSolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void three_Checked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Add(2);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void three_Unchecked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Remove(2);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void two_Checked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Add(1);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void two_Unchecked(object sender, RoutedEventArgs e)
        {
            Rack.NetychordsDMIBox.reeds.Remove(1);
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.StringToChordFactory(Rack.NetychordsDMIBox.firstNote, "2", ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        // [Corrente]
        private void UpdateSensorConnection() 
        {
            txtSensorPort.Text = "COM" + SensorPort.ToString();

            if (Rack.NetychordsDMIBox.HeadTrackerModule.Connect(SensorPort))
            {
                txtSensorPort.Foreground = ActiveBrush;
            }
            else
            {
                txtSensorPort.Foreground = WarningBrush;
            }
        }

        /// <summary>
        /// This method gets called every millisecond (or something like?) in order to update the
        /// elements of the GUI
        /// </summary>
        private void UpdateWindow(object sender, EventArgs e)
        {
            if (netychordsStarted)
            {
                lblIsPlaying.Text = Rack.NetychordsDMIBox.isPlaying;
                lblPlayedNote.Text = Rack.NetychordsDMIBox.Chord.ChordName();
                lblYaw.Text = Rack.NetychordsDMIBox.HTData.TranspYaw.ToString();
                centerValue.Text = Math.Round(Rack.NetychordsDMIBox.CenterZone, 0).ToString();
                centerPitchValue.Text = Math.Round(centerPitchZone.Value, 0).ToString();

                Rack.NetychordsDMIBox.NetychordsSurface.UpdateHeadTrackerFeedback(Rack.NetychordsDMIBox.HTData);
            }

            if (clickedButton)
            {
                TimeSpan limit = new TimeSpan(0, 0, 0, 0, 30);
                TimeSpan button = DateTime.Now.Subtract(clicked);
                if (button >= limit)
                {
                    btnMIDIchMinus.IsEnabled = true;
                    btnMIDIchPlus.IsEnabled = true;
                    btnSensorPortMinus.IsEnabled = true;
                    btnSensorPortPlus.IsEnabled = true;
                    clickedButton = false;
                }
            }
        }
    }
}