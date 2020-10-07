using Netytar.DMIbox;
using System;
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

        private bool NetychordsStarted = false;
        
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
            if (NetychordsStarted)
            {
               // Put here all the stuff which needs to be updated!
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
            NetychordsStarted = true; 
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
            if (NetychordsStarted)
            {
                Rack.NetytarDMIBox.MidiModule.OutDevice--;
                lblMIDIch.Text = "MP" + Rack.NetytarDMIBox.MidiModule.OutDevice.ToString();

                CheckMidiPort();
            }
        }

        private void btnMIDIchPlus_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                Rack.NetytarDMIBox.MidiModule.OutDevice++;
                lblMIDIch.Text = "MP" + Rack.NetytarDMIBox.MidiModule.OutDevice.ToString();

                CheckMidiPort();
            }
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string noteName = (string)button.Content;
            int octaveNumber = 4;
            NetychordsDMIBox.MidiChord chord = NetychordsDMIBox.StringToNote(noteName, octaveNumber);
            //NetychordsDMIBox.ChordType chordtype = NetychordsDMIBox.ChordType.Major;
            if (Rack.NetychordsDMIBox.playing == true)
            {
                Rack.NetychordsDMIBox.StopSelectedChord(Rack.NetychordsDMIBox.lastChord);
            };

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Space))
            {
                Rack.NetychordsDMIBox.PlaySelectedChord(chord);
            }
            else if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.Space))
            {
                Rack.NetychordsDMIBox.StopSelectedChord(chord);
            }
        }


        private void tabSolo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string noteName = (string)button.Content;
            MidiNotes note = (MidiNotes)Enum.Parse(typeof(MidiNotes), noteName);
            Rack.NetychordsDMIBox.PlaySelectedChordMajor(note);*/
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*System.Windows.Controls.Button button = (System.Windows.Controls.Button)sender;
            string noteName = (string)button.Content;
            MidiNotes note = (MidiNotes)Enum.Parse(typeof(MidiNotes), noteName);
            Rack.NetychordsDMIBox.StopSelectedChordMajor(note);*/
        }

        private void CanvasNetytchords_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key == System.Windows.Input.Key.Space) && Rack.NetychordsDMIBox.playing == true)
            {
                Rack.NetychordsDMIBox.StopSelectedChord(Rack.NetychordsDMIBox.lastChord);
            };
        }

        private void canvasNetytchords_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (System.Windows.Input.Keyboard.IsKeyUp(System.Windows.Input.Key.Space) && Rack.NetychordsDMIBox.playing == true)
            {
                Rack.NetychordsDMIBox.StopSelectedChord(Rack.NetychordsDMIBox.lastChord);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}