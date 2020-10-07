using NeeqDMIs.ATmega;
using NeeqDMIs.Music;
using NeeqDMIs.Keyboard;
using Netytar.DMIbox;
using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Tobii.Interaction;
using Tobii.Interaction.Wpf;
using RawInputProcessor;


namespace Netytar
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int breathSensorValue = 0;
        public int BreathSensorValue { get => breathSensorValue; set => breathSensorValue = value; }

        private WpfInteractorAgent wpfInteractorAgent;

        private Scale StartingScale = ScalesFactory.Cmaj;

        private Scale lastScale;
        private Scale selectedScale;
        public Scale SelectedScale { get => selectedScale; set => selectedScale = value; }

        private const int BreathMax = 340;

        private readonly SolidColorBrush ActiveBrush = new SolidColorBrush(Colors.LightGreen);
        private readonly SolidColorBrush WarningBrush = new SolidColorBrush(Colors.DarkRed);
        private readonly SolidColorBrush BlankBrush = new SolidColorBrush(Colors.Black);

        private int sensorPort = 11;
        public int SensorPort
        {
            get { return sensorPort; }
            set
            {
                if(value > 0)
                {
                    sensorPort = value;
                }
            }
        }

        public WpfInteractorAgent WpfInteractorAgent { get => wpfInteractorAgent; set => wpfInteractorAgent = value; }

        private bool NetychordsStarted = false;
        
        private Timer updater;

        private double velocityBarMaxHeight = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            updater = new Timer();
            updater.Interval = 10;
            updater.Tick += UpdateWindow;
            updater.Start();

            lastScale = StartingScale;
            SelectedScale = StartingScale;
            
            //Behaviors.SetIsGazeAware(btnNetytarSelect, true);
            //Behaviors.AddHasGazeChangedHandler(btnNetytarSelect, eyeGazeHandler);
            
        }

        private void eyeGazeHandler(object sender, HasGazeChangedRoutedEventArgs e)
        {
            if (e.HasGaze)
            {
                Rack.NetytarDMIBox.HasAButtonGaze = true;
                Rack.NetytarDMIBox.LastGazedButton = (System.Windows.Controls.Button)sender;
            }
            else
            {
                Rack.NetytarDMIBox.HasAButtonGaze = false;
            }
        }

        private void UpdateWindow(object sender, EventArgs e)
        {
            if (NetychordsStarted)
            {
                VelocityBar.Height = (velocityBarMaxHeight * breathSensorValue) / BreathMax;

                if (SelectedScale.GetName().Equals(lastScale.GetName()) == false)
                {
                    lastScale = selectedScale;
                    Rack.NetytarDMIBox.NetytarSurface.Scale = selectedScale;
                }

                txtNoteName.Text = Rack.NetytarDMIBox.SelectedNote.ToStandardString();
                txtPitch.Text = Rack.NetytarDMIBox.SelectedNote.ToPitchValue().ToString();
                if (Rack.NetytarDMIBox.Blow)
                {
                    txtIsBlowing.Text = "B";
                }
                else
                {
                    txtIsBlowing.Text = "_";
                }

                /*
                try
                {
                    txtEyePosX.Text = NetytarRack.DMIBox.EyeXModule.LastEyePosition.LeftEye.X.ToString();
                    txtEyePosY.Text = NetytarRack.DMIBox.EyeXModule.LastEyePosition.LeftEye.Y.ToString();
                    txtEyePosZ.Text = NetytarRack.DMIBox.EyeXModule.LastEyePosition.LeftEye.Z.ToString();
                }
                catch
                {

                }*/

                txtTest.Text = Rack.NetytarDMIBox.TestString;

            }
        }

        private void StartNetytar(object sender, RoutedEventArgs e)
        {
            AddScaleListItems();

            //NetytarSetup netytarSetup = new NetytarSetup(this);
            //netytarSetup.Setup();

            NetychordsSetup netychordsSetup = new NetychordsSetup(this);
            netychordsSetup.Setup();

            //wpfInteractorAgent = NetytarRack.DMIBox.TobiiModule.TobiiHost.InitializeWpfAgent();

            //InitializeVolumeBar();
            //InitializeSensorPortText();

            //if (Rack.NetytarDMIBox.NetytarControlMode == NetytarControlModes.Keyboard)
            //{
            //    indCtrlKeyboard.Background = ActiveBrush;
            //}

            //if (Rack.NetytarDMIBox.NetytarControlMode == NetytarControlModes.BreathSensor)
            //{
            //    indCtrlBreath.Background = ActiveBrush;
            //}

            btnStart.IsEnabled = false;
            btnStart.Foreground = new SolidColorBrush(Colors.Black);

            CheckMidiPort();

            breathSensorValue = 0;

            //UpdateIndicators();

            NetychordsStarted = true; // LEAVE AT THE END!
        }

        private void UpdateIndicators()
        {
            switch (Rack.NetytarDMIBox.NetytarControlMode)
            {
                case NetytarControlModes.BreathSensor:
                    indCtrlKeyboard.Background = BlankBrush;
                    indCtrlBreath.Background = ActiveBrush;
                    indCtrlEyePos.Background = BlankBrush;
                    indCtrlEyeVel.Background = BlankBrush;
                    break;
                case NetytarControlModes.EyePos:
                    indCtrlKeyboard.Background = BlankBrush;
                    indCtrlBreath.Background = BlankBrush;
                    indCtrlEyePos.Background = ActiveBrush;
                    indCtrlEyeVel.Background = BlankBrush;
                    break;
                case NetytarControlModes.Keyboard:
                    indCtrlKeyboard.Background = ActiveBrush;
                    indCtrlBreath.Background = BlankBrush;
                    indCtrlEyePos.Background = BlankBrush;
                    indCtrlEyeVel.Background = BlankBrush;
                    break;
                case NetytarControlModes.EyeVel:
                    indCtrlKeyboard.Background = BlankBrush;
                    indCtrlBreath.Background = BlankBrush;
                    indCtrlEyePos.Background = BlankBrush;
                    indCtrlEyeVel.Background = ActiveBrush;
                    break;
            }
            switch (Rack.NetytarDMIBox.ModulationControlMode)
            {
                case ModulationControlModes.On:
                    indModulationControl.Background = ActiveBrush;
                    break;
                case ModulationControlModes.Off:
                    indModulationControl.Background = BlankBrush;
                    break;
            }
            switch (Rack.NetytarDMIBox.BreathControlMode)
            {
                case BreathControlModes.Switch:
                    indBreathSwitch.Background = ActiveBrush;
                    break;
                case BreathControlModes.Dynamic:
                    indBreathSwitch.Background = BlankBrush;
                    break;
            }
        }

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

        private void InitializeSensorPortText()
        {
            txtSensorPort.Foreground = WarningBrush;
            txtSensorPort.Text = Rack.NetytarDMIBox.SensorReader.PortPrefix + SensorPort;
            UpdateSensorConnection();
        }

        private void InitializeVolumeBar()
        {
            velocityBarMaxHeight = VelocityBarBorder.ActualHeight;
            VelocityBar.Height = 0;
            MaxBar.Height = VelocityBar.Height = (velocityBarMaxHeight * 127) / BreathMax;
        }

        private void Test(object sender, RoutedEventArgs e)
        {
            Rack.NetytarDMIBox.NetytarSurface.DrawScale();
        }

        private void AddScaleListItems()
        {
            foreach (Scale scale in ScalesFactory.GetList())
            {
                ListBoxItem item = new ListBoxItem() { Content = scale.GetName() };
                lstScaleChanger.Items.Add(item);
            }
        }

        internal void ChangeScale(ScaleCodes scaleCode)
        {
            Rack.NetytarDMIBox.NetytarSurface.Scale = new Scale(Rack.NetytarDMIBox.SelectedNote.ToAbsNote(), scaleCode); 
        }

        public void ReceiveNoteChange()
        {
            txtPitch.Text = Rack.NetytarDMIBox.SelectedNote.ToPitchValue().ToString();
            txtNoteName.Text = Rack.NetytarDMIBox.SelectedNote.ToStandardString();
        }

        public void ReceiveBlowingChange()
        {
            if (Rack.NetytarDMIBox.Blow)
            {
                txtIsBlowing.Text = "B";
            }
            else
            {
                txtIsBlowing.Text = "_";
            }
        }

        private void BtnScroll_Click(object sender, RoutedEventArgs e)
        {
            Rack.NetytarDMIBox.AutoScroller.Enabled = !Rack.NetytarDMIBox.AutoScroller.Enabled;
        }

        private void BtnFFBTest_Click(object sender, RoutedEventArgs e)
        {
            //Rack.DMIBox.FfbModule.FlashFFB();
        }

        private void LstScaleChanger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rack.NetytarDMIBox.NetytarSurface.Scale = Scale.FromString(((ListBoxItem)lstScaleChanger.SelectedItem).Content.ToString());
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

        private void btnCtrlKeyboard_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                Rack.NetytarDMIBox.NetytarControlMode = NetytarControlModes.Keyboard;
                Rack.NetytarDMIBox.ResetModulationAndPressure();

                breathSensorValue = 0;

                UpdateIndicators();
            }
        }

        private void btnCtrlBreath_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                Rack.NetytarDMIBox.NetytarControlMode = NetytarControlModes.BreathSensor;
                Rack.NetytarDMIBox.ResetModulationAndPressure();

                breathSensorValue = 0;

                UpdateIndicators();
            }
        }

        private void btnSensorPortPlus_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                SensorPort++;
                UpdateSensorConnection();
            }
        }

        private void UpdateSensorConnection()
        {
            txtSensorPort.Text = Rack.NetytarDMIBox.SensorReader.PortPrefix + SensorPort.ToString();

            if (Rack.NetytarDMIBox.SensorReader.Connect(SensorPort))
            {
                txtSensorPort.Foreground = ActiveBrush;
            }
            else
            {
                txtSensorPort.Foreground = WarningBrush;
            }
        }

        private void btnSensorPortMinus_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                SensorPort--;
                UpdateSensorConnection();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Rack.NetytarDMIBox.Dispose();
            Close();
        }

        private void btnCtrlEyePos_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                Rack.NetytarDMIBox.NetytarControlMode = NetytarControlModes.EyePos;
                Rack.NetytarDMIBox.ResetModulationAndPressure();

                breathSensorValue = 0;

                UpdateIndicators();
            }
        }



        private void btnExit_Activate(object sender, RoutedEventArgs e)
        {

        }

        
        private void btnCalibrateHeadPose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (NetychordsStarted)
            {
                btnCalibrateHeadPose.Background = new SolidColorBrush(Colors.LightGreen);

                if(Rack.NetytarDMIBox.TobiiModule.LastHeadPoseData != null && Rack.NetytarDMIBox.TobiiModule.LastHeadPoseData.HasHeadPosition)
                {
                    Rack.NetytarDMIBox.HeadPoseBaseX = Rack.NetytarDMIBox.TobiiModule.LastHeadPoseData.HeadRotation.X;
                    Rack.NetytarDMIBox.HeadPoseBaseY = Rack.NetytarDMIBox.TobiiModule.LastHeadPoseData.HeadRotation.Y;
                    Rack.NetytarDMIBox.HeadPoseBaseZ = Rack.NetytarDMIBox.TobiiModule.LastHeadPoseData.HeadRotation.Z;
                }

                Rack.NetytarDMIBox.CalibrateGyroBase();
                Rack.NetytarDMIBox.CalibrateAccBase();


            }
        }

        
        private void btnCalibrateHeadPose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btnCalibrateHeadPose.Background = new SolidColorBrush(Colors.Black);
        }

        private void btnTestClick(object sender, RoutedEventArgs e)
        {
            throw (new NotImplementedException("Test button is not set!"));
        }

        private void btnModulationControlSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                if (Rack.NetytarDMIBox.ModulationControlMode == ModulationControlModes.Off)
                {
                    Rack.NetytarDMIBox.ModulationControlMode = ModulationControlModes.On;
                }
                else if (Rack.NetytarDMIBox.ModulationControlMode == ModulationControlModes.On)
                {
                    Rack.NetytarDMIBox.ModulationControlMode = ModulationControlModes.Off;
                }
            }

            breathSensorValue = 0;

            UpdateIndicators();
        }

        private void btnBreathControlSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                if (Rack.NetytarDMIBox.BreathControlMode == BreathControlModes.Switch)
                {
                    Rack.NetytarDMIBox.BreathControlMode = BreathControlModes.Dynamic;
                }
                else if (Rack.NetytarDMIBox.BreathControlMode == BreathControlModes.Dynamic)
                {
                    Rack.NetytarDMIBox.BreathControlMode = BreathControlModes.Switch;
                }
            }

            breathSensorValue = 0;

            UpdateIndicators();
        }

        private void btnCtrlEyeVel_Click(object sender, RoutedEventArgs e)
        {
            if (NetychordsStarted)
            {
                Rack.NetytarDMIBox.NetytarControlMode = NetytarControlModes.EyeVel;
                Rack.NetytarDMIBox.ResetModulationAndPressure();

                breathSensorValue = 0;

                UpdateIndicators();
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
    }
}