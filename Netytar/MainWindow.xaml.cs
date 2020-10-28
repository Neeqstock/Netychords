using NeeqDMIs.Music;
using Netytar.DMIBox;
using Netytar.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Netytar
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);

        private bool netychordsStarted = false;
        
        private DispatcherTimer updater;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Initializing dispatcher timer, i.e. the timer that updates every graphical value in the interface.
            updater = new DispatcherTimer();
            updater.Interval = new TimeSpan(1000);
            updater.Tick += UpdateWindow;
            updater.Start();
        }

        /// <summary>
        /// This method gets called every millisecond (or something like?) in order to update the elements of the GUI
        /// </summary>
        private void UpdateWindow(object sender, EventArgs e)
        {
            if (netychordsStarted)
            {
                lblIsPlaying.Text = Rack.NetychordsDMIBox.isPlaying;
                lblPlayedNote.Text = Rack.NetychordsDMIBox.Chord.rootNote.ToStandardString();
                
            }
        }

        /// <summary>
        /// This gets called when the Start button is pressed
        /// </summary>
        private void StartNetytar(object sender, RoutedEventArgs e)
        {
            // Launches the Setup class
            NetychordsSetup netychordsSetup = new NetychordsSetup(this);
            netychordsSetup.Setup();

            // Changes the aspect of the Start button
            btnStart.IsEnabled = false;
            btnStart.Foreground = new SolidColorBrush(Colors.Black);

            // Checks the selected MIDI port is available
            CheckMidiPort();

            // LEAVE AT THE END! This keeps track of the started state
            netychordsStarted = true; 
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

        private void btnMIDIchMinus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.NetytarDMIBox.MidiModule.OutDevice--;
                lblMIDIch.Text = "MP" + Rack.NetytarDMIBox.MidiModule.OutDevice.ToString();

                CheckMidiPort();
            }
        }

        private void btnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (netychordsStarted)
            {
                Rack.NetytarDMIBox.MidiModule.OutDevice++;
                lblMIDIch.Text = "MP" + Rack.NetytarDMIBox.MidiModule.OutDevice.ToString();

                CheckMidiPort();
            }
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (netychordsStarted)
            {
                //DMIBox.SelectedChord = Contenuto del bottone;

                System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
                string noteName = (string)button.Content;
                //int octaveNumber = 4;
                //Rack.DMIBox.Chord = MidiChord.StringToNote(noteName, Rack.DMIBox.octaveNumber);
                //DMIBox.ChordType chordtype = DMIBox.ChordType.Major;
                /*if (Rack.DMIBox.playing == true)
                {
                    Rack.DMIBox.StopChord(Rack.DMIBox.lastChord);
                };*/

                
            }
        }
        // [Corrente]

        private void tabSolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string noteName = (string)button.Content;
            MidiNotes note = (MidiNotes)Enum.Parse(typeof(MidiNotes), noteName);
            Rack.DMIBox.PlaySelectedChordMajor(note);*/
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string noteName = (string)button.Content;
            MidiNotes note = (MidiNotes)Enum.Parse(typeof(MidiNotes), noteName);
            Rack.DMIBox.StopSelectedChordMajor(note);*/
        }


        private void canvasNetytchords_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*if (true == false)
            {
                if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.Space) && Rack.DMIBox.playing == true)
                {
                    Rack.DMIBox.StopChord(Rack.DMIBox.lastChord);
                }
            }*/
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LstOctaveChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rack.NetychordsDMIBox.octaveNumber = ((ListBoxItem)lstOctaveChanger.SelectedItem).Content.ToString();
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.ChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void lstNoteChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rack.NetychordsDMIBox.firstNote = ((ListBoxItem)lstNoteChanger.SelectedItem).Content.ToString();
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.ChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void Margins_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (netychordsStarted)
            {
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.ChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void lstLayout_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rack.NetychordsDMIBox.layout = ((ListBoxItem)lstLayout.SelectedItem).Content.ToString();

            if (Rack.NetychordsDMIBox.layout == "Arbitrary")
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

            if (netychordsStarted && Rack.NetychordsDMIBox.layout != "Arbitrary")
            {
                Rack.NetychordsDMIBox.arbitraryLines = new List<string>();

                arbitraryStart.IsEnabled = false;
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.ChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
            }
        }

        private void SetupComboBox()
        {
            System.Collections.Generic.List<ComboBox> boxes = new System.Collections.Generic.List<ComboBox> { FirstRow, SecondRow, ThirdRow, FourthRow, FifthRow, SixthRow, SeventhRow, EighthRow, NinthRow, TenthRow, EleventhRow };
            for (int i = 0; i < 11; i++)
            {
                for (int j=0; j < 11; j++)
                {
                    boxes[i].Items.Add(((ChordType)j).ToString());
                }
            }
            

        }


        private void selectorRow_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void arbitraryStart_Click(object sender, RoutedEventArgs e)
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
                Rack.NetychordsDMIBox.NetychordsSurface.firstChord = MidiChord.ChordFactory(Rack.NetychordsDMIBox.firstNote, Rack.NetychordsDMIBox.octaveNumber, ChordType.Major);
                canvasNetychords.Children.Clear();
                Rack.NetychordsDMIBox.NetychordsSurface.DrawButtons();
                canvasNetychords.Children.Add(Rack.NetychordsDMIBox.NetychordsSurface.highlighter);
                arbitraryStart.IsEnabled = false;
            }
        }
    }
}