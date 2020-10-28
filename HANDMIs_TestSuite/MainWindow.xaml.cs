using HANDMIsTestSuite.Modules;
using HANDMIsTestSuite.Modules.GuideModule;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HANDMIsTestSuite
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatcherTimerX;
        private DispatcherTimer monitorTimer;

        private Rectangle rctXTarget;
        private Button XCursor;
        private Rectangle rctXTargetCenter;
        Random random = new Random();

        private void UpdateHeadTrackerConnection()
        {
            txtHTPortNumber.Text = Rack.DMIBox.HeadTrackerPortNumber.ToString();

            if (Rack.DMIBox.HeadTrackerModule.Connect(Rack.DMIBox.HeadTrackerPortNumber))
            {
                txtHTPortNumber.Foreground = GlobalValues.ColorActive;
            }
            else
            {
                txtHTPortNumber.Foreground = GlobalValues.ColorError;
            }
        }

        private void UpdateBreathSensorConnection()
        {
            txtBSPortNumber.Text = Rack.DMIBox.BreathSensorPortNumber.ToString();

            if (Rack.DMIBox.BreathSensorModule.Connect(Rack.DMIBox.BreathSensorPortNumber))
            {
                txtBSPortNumber.Foreground = GlobalValues.ColorActive;
            }
            else
            {
                txtBSPortNumber.Foreground = GlobalValues.ColorError;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Style s = new Style();
            s.Setters.Add(new Setter(VisibilityProperty, Visibility.Collapsed));
            tabControl.ItemContainerStyle = s;

            monitorTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = new TimeSpan(100000)
            };

            monitorTimer.Tick += MonitorTimer_Tick;

        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            tbxMHeadTrackerRaw.Text = Rack.DMIBox.Str_HeadTrackerRaw;
            tbxMHeadTrackerCalib.Text = Rack.DMIBox.Str_HeadTrackerCalib;
            tbxMBreathSensor.Text = Rack.DMIBox.BreathSensorValue.ToString() + "\n" + Rack.DMIBox.BreathSensorZero.ToString() + "\n" + Rack.DMIBox.BreathSensorCalib.ToString() + "\n" + Rack.DMIBox.BreathSensorMax.ToString();
            
            sldYaw.Value = Rack.DMIBox.HeadTrackerData.TranspYaw;
            sldPitch.Value = Rack.DMIBox.HeadTrackerData.TranspPitch;
            sldRoll.Value = Rack.DMIBox.HeadTrackerData.TranspRoll;


            if (Rack.DMIBox.EyeTribeGPData != null)
            {
                tbxMEyeTribeGPRaw.Text = Rack.DMIBox.EyeTribeGPData.RawCoordinates.X.ToString() + "\n" + Rack.DMIBox.EyeTribeGPData.RawCoordinates.Y.ToString();
                tbxMEyeTribeGPSmooth.Text = Rack.DMIBox.EyeTribeGPData.SmoothedCoordinates.X.ToString() + "\n" + Rack.DMIBox.EyeTribeGPData.SmoothedCoordinates.Y.ToString();
            }

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            Application.Current.Shutdown();
        }

        private void BtnGenerateXTarget_Click(object sender, RoutedEventArgs e)
        {
            GenerateXTarget();
            GenerateXCursor();

            Rack.DMIBox.GuideModule.Reset();
            Rack.DMIBox.GuideModule.AddString("Yeah", 6, 0xFF, 0xFF, 0xFF);
            Rack.DMIBox.GuideModule.AddString("Target generated!", 6, 0xFF, 0xFF, 0xFF);
            Rack.DMIBox.GuideModule.StartStrings();
        }

        private void GenerateXTarget()
        {
            if (rctXTarget != null)
            {
                XObjectsCanvas.Children.Remove(rctXTarget);
            }

            if (rctXTargetCenter != null)
            {
                XObjectsCanvas.Children.Remove(rctXTargetCenter);
            }

            rctXTarget = new Rectangle
            {
                Fill = GlobalValues.ColorTargetNotLocked,
                Height = rctXBar.Height,
                Width = GlobalValues.TargetWidth
            };

            XObjectsCanvas.Children.Add(rctXTarget);
            Panel.SetZIndex(rctXTarget, 50);

            Canvas.SetTop(rctXTarget, (double)rctXBar.GetValue(Canvas.TopProperty));
            Canvas.SetLeft(rctXTarget, (double)rctXBar.GetValue(Canvas.LeftProperty));

            rctXTargetCenter = new Rectangle
            {
                Fill = GlobalValues.ColorTargetCenterNotLocked,
                Height = rctXBar.Height + (GlobalValues.TargetCenterOffset * 2),
                Width = GlobalValues.TargetCenterWidth
            };

            XObjectsCanvas.Children.Add(rctXTargetCenter);
            Panel.SetZIndex(rctXTargetCenter, 1);

            SetTargetCenter();

            Rack.DMIBox.XTargetLeft = (int)Canvas.GetLeft(rctXTarget);
        }

        private void SetTargetCenter()
        {
            Canvas.SetTop(rctXTargetCenter, (double)rctXTarget.GetValue(Canvas.TopProperty) - GlobalValues.TargetCenterOffset);
            Canvas.SetLeft(rctXTargetCenter, (double)rctXTarget.GetValue(Canvas.LeftProperty) + (GlobalValues.TargetWidth / 2) - (GlobalValues.TargetCenterWidth / 2));
        }

        private void GenerateXCursor()
        {
            if (XCursor != null)
            {
                XObjectsCanvas.Children.Remove(XCursor);
            }

            XCursor = new Button
            {
                Background = GlobalValues.ColorCursor,
                Height = rctXBar.Height + GlobalValues.CursorOffset,
                Width = GlobalValues.CursorWidth
            };

            XObjectsCanvas.Children.Add(XCursor);
            Panel.SetZIndex(XCursor, 100);

            Canvas.SetTop(XCursor, (double)rctXBar.GetValue(Canvas.TopProperty) - (GlobalValues.CursorOffset / 2));
            Canvas.SetLeft(XCursor, (double)rctXBar.GetValue(Canvas.LeftProperty));
        }

        private void RandomizeXTargetPosition()
        {
            double newPosition = (double)rctXBar.GetValue(Canvas.LeftProperty) + random.Next(0, (int)rctXBar.ActualWidth - (int)rctXTarget.ActualWidth);
            Canvas.SetTop(rctXTarget, (double)rctXBar.GetValue(Canvas.TopProperty));
            Canvas.SetLeft(rctXTarget, newPosition);

            SetTargetCenter();

            Rack.DMIBox.XTargetLeft = (int)newPosition;
        }

        private void PlaceTargetAccordingToFile()
        {
            double newPosition = (double)rctXBar.GetValue(Canvas.LeftProperty) + Rack.DMIBox.TargetDistances[Rack.DMIBox.DistancesIndex].Distance - GlobalValues.TargetWidth / 2;
            Rack.DMIBox.DistancesIndex++;
            
            Canvas.SetTop(rctXTarget, (double)rctXBar.GetValue(Canvas.TopProperty));
            Canvas.SetLeft(rctXTarget, newPosition);

            SetTargetCenter();

            Rack.DMIBox.XTargetLeft = (int)newPosition;
        }

        private void SetXTargetToRest()
        {
            double newPosition = (double)rctXBar.GetValue(Canvas.LeftProperty) - rctXTarget.Width / 2;
            Canvas.SetTop(rctXTarget, (double)rctXBar.GetValue(Canvas.TopProperty));
            Canvas.SetLeft(rctXTarget, newPosition);

            SetTargetCenter();

            Rack.DMIBox.XTargetLeft = (int)newPosition;
        }

        private void TabControl_Loaded(object sender, RoutedEventArgs e)
        {
            TestSuiteSetup setup = new TestSuiteSetup(this);
            setup.Setup();

            UpdateHeadTrackerConnection();
            UpdateBreathSensorConnection();

            txtXGuide.Text = "Hello!";

            GenerateTabX();

            dispatcherTimerX = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = new TimeSpan(100000)
            };


            dispatcherTimerX.Tick += DispatcherTimerX_Tick;

            Rack.DMIBox.PollingModuleStopwatch = new PollingModuleStopwatch(null, null);
        }

        private void InitializeTest()
        {
            Rack.DMIBox.PrinterModule.ReadTargetDistancesFile();
            Rack.DMIBox.TargetDistances.Shuffle();

            Rack.DMIBox.GuideModule.Reset();
            Rack.DMIBox.GuideModule.AddStrings(GlobalValues.GuideExperimentStart);
            Rack.DMIBox.GuideModule.StartStrings();
            Rack.DMIBox.GuideModule.onFinish += new OnFinish(TestFirstCall); //StartFittsTest
            Rack.DMIBox.TrialNumber = 0;

            tbxSubjectName.IsReadOnly = true;
            tbxSubjectName.Foreground = GlobalValues.ColorError;
            Rack.DMIBox.SubjectName = tbxSubjectName.Text;
        }

        private void TestFirstCall()
        {
            Rack.DMIBox.TestStarted = true;
            Rack.DMIBox.TestState = TestStates.RestWaitInput;
        }

        private void DispatcherTimerX_Tick(object sender, EventArgs e)
        {
            // START NEW PHASES ==========================
            
            switch (Rack.DMIBox.TestState) //TODO MODIFICARE SOLO IN WAITFORFITTS
            {
                case TestStates.WaitForFitts:
                    StartFittsPhase();
                    break;
                case TestStates.WaitForRestChallenge:
                    StartRestChallengePhase();
                    break;
                case TestStates.Fitts:
                    break;
                case TestStates.RestWaitInput:
                    if(Rack.DMIBox.DistancesIndex >= Rack.DMIBox.TargetDistances.Count)
                    {
                        ProcessTestEnd();
                    }
                    rctXTargetCenter.Fill = rctXTarget.Fill = GlobalValues.ColorTransparent;
                    break;
                case TestStates.RestChallenge:
                    break;
                case TestStates.Printing:
                    break;
                case TestStates.Stop:
                    break;
                case TestStates.Pause:
                    break;
                default:
                    break;
            }

            // UPDATE GRAPHICS ===========================

            if (Rack.DMIBox.TestStarted)
            {
                Canvas.SetLeft(XCursor, Rack.DMIBox.XCursorLeft);
                
                if(Rack.DMIBox.TestState != TestStates.RestWaitInput)
                {
                    switch (Rack.DMIBox.Locked)
                    {
                        case LockedStates.Locked:
                            rctXTarget.Fill = GlobalValues.ColorTargetLocked;
                            rctXTargetCenter.Fill = GlobalValues.ColorTargetCenterLocked;
                            break;
                        case LockedStates.Not:
                            rctXTarget.Fill = GlobalValues.ColorTargetNotLocked;
                            rctXTargetCenter.Fill = GlobalValues.ColorTargetCenterNotLocked;
                            break;
                        case LockedStates.Selected:
                            rctXTarget.Fill = GlobalValues.ColorTargetSelected;
                            rctXTargetCenter.Fill = GlobalValues.ColorTargetCenterSelected;
                            break;
                    }
                }  
            }
        }

        private void ProcessTestEnd()
        {
            StopAll();
            Rack.DMIBox.GuideModule.Reset();
            Rack.DMIBox.GuideModule.AddStrings(GlobalValues.GuideExperimentFinish);
            Rack.DMIBox.GuideModule.StartStrings();
        }

        private void BtnRandomizeXTargetPos_Click(object sender, RoutedEventArgs e)
        {
            if (rctXTarget != null)
            {
                RandomizeXTargetPosition();
            }
        }

        private void BtnControlMouseX_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.MouseX;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void StartXTest()
        {
            Rack.DMIBox.TestState = TestStates.Stop;
            GenerateXTarget();
            GenerateXCursor();
            InitializeTest();
        }

        public void StartFittsPhase()
        {
            Rack.DMIBox.TestStarted = false;
            Rack.DMIBox.TrialNumber++;

            if (Rack.DMIBox.TrialNumber <= GlobalValues.TrainingTrialsNumber)
            {
                txtTrialNumber.Foreground = GlobalValues.ColorTrialsTraining;
                RandomizeXTargetPosition();
            }
            else
            {
                txtTrialNumber.Foreground = GlobalValues.ColorTrialsMeasured;
                PlaceTargetAccordingToFile();
            }
            txtTrialNumber.Text = Rack.DMIBox.TrialNumber.ToString();

            GlobalValues.SoundTrialStart.Play();
            Rack.DMIBox.PollingModuleStopwatch.StartFitts();

            Rack.DMIBox.TestStarted = true;
        }

        private void StartRestChallengePhase()
        {
            // TODO suono
            Rack.DMIBox.TestStarted = false;

            txtTrialNumber.Foreground = GlobalValues.ColorTrialsRest;
            txtTrialNumber.Text = Rack.DMIBox.TrialNumber.ToString(); // UNNECESSARY

            GlobalValues.SoundRestInput.Play();

            SetXTargetToRest();

            Rack.DMIBox.PollingModuleStopwatch.StartRestChallenge();
            Rack.DMIBox.TestStarted = true;
        }

        private void btnStopAll_Click(object sender, RoutedEventArgs e)
        {
            StopAll();
        }

        private void StopAll()
        {
            Rack.DMIBox.TestStarted = false;
            Rack.DMIBox.ControlMode = ControlModes.None;
            dispatcherTimerX.Stop();
            Rack.DMIBox.GuideModule.Reset();
            Rack.DMIBox.PollingModuleStopwatch?.StopFitts();
            Rack.DMIBox.TrialNumber = 0;
            Rack.DMIBox.DistancesIndex = 0;
            ResetTxtTrialNumber();
            tbxSubjectName.IsReadOnly = false;
            tbxSubjectName.Foreground = new SolidColorBrush(Colors.White);
            Rack.DMIBox.TestState = TestStates.Stop;

            //MessageBox.Show(System.Windows.SystemParameters.PrimaryScreenWidth.ToString());
        }

        private void ResetTxtTrialNumber()
        {
            txtTrialNumber.Text = "-";
            txtTrialNumber.Foreground = new SolidColorBrush(Colors.White);
        }

        private void btnHTPortMinus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.HeadTrackerPortNumber--;
            UpdateHeadTrackerConnection();
        }

        private void btnHTPortPlus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.HeadTrackerPortNumber++;
            UpdateHeadTrackerConnection();
        }

        private void btnXTab_Click(object sender, RoutedEventArgs e)
        {
            GenerateTabX();
            tabControl.SelectedItem = tabX;
        }

        private void tabX_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateTabX();
        }

        private void GenerateTabX()
        {
            Canvas.SetLeft(rctXBar, (XObjectsCanvas.ActualWidth - rctXBar.ActualWidth) / 2);
            Canvas.SetTop(rctXBar, (XObjectsCanvas.ActualHeight - rctXBar.ActualHeight) / 2);
            Rack.DMIBox.XBarLeft = Canvas.GetLeft(rctXBar);
            Rack.DMIBox.XBarRight = Canvas.GetLeft(rctXBar) + rctXBar.ActualWidth;

            Canvas.SetLeft(txtXGuide, Rack.DMIBox.XBarLeft);
            Canvas.SetTop(txtXGuide, Canvas.GetTop(rctXBar) + rctXBar.ActualHeight + GlobalValues.BarTextSeparator);
        }

        private void btnControlGazePointXRaw_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.GazePointXRaw;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnControlBreathIntensity_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.BreathIntensity;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnControlHeadYaw_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.HeadYaw;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnDebugA_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.TestState = TestStates.RestWaitInput;
            Rack.DMIBox.TrialNumber--;
            if(Rack.DMIBox.DistancesIndex != 0)
            {
                Rack.DMIBox.DistancesIndex--;
            }
        }

        private void btnDebugB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnControlHeadPitch_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.HeadPitch;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnMonitorTab_Click(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedItem = tabMonitor;
        }

        private void btnMonitorStart_Click(object sender, RoutedEventArgs e)
        {
            monitorTimer.Start();
        }

        private void btnMonitorStop_Click(object sender, RoutedEventArgs e)
        {
            monitorTimer.Stop();
        }

        private void btnCalibrateHeadTracker_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.HeadTrackerData.SetDeltaForAll();
        }

        private void btnCalibrateBreathSensor_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.BreathSensorZero = Rack.DMIBox.BreathSensorValue;
        }

        private void btnBSPortPlus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.BreathSensorPortNumber++;
            UpdateBreathSensorConnection();
        }

        private void btnBSPortMinus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.BreathSensorPortNumber--;
            UpdateBreathSensorConnection();
        }

        private void btnControlGazePointXSmooth_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.GazePointXSmooth;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnControlHeadRoll_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.HeadRoll;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnControlHeadVelocity_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.ControlMode = ControlModes.HeadVelocity;
            StartXTest();
            dispatcherTimerX.Start();
        }

        private void btnRollMinus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.RollInverter = -1;
        }

        private void btnRollPlus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.RollInverter = 1;
        }

        private void btnPitchPlus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.PitchInverter = -1;
        }

        private void btnPitchMinus_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.PitchInverter = 1;
        }

        private void btnInvertPitchRoll_Click(object sender, RoutedEventArgs e)
        {
            Rack.DMIBox.InvertPitchRoll = !Rack.DMIBox.InvertPitchRoll;
        }
    }
}
