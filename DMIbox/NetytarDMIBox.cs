using NeeqDMIs;
using NeeqDMIs.ATmega;
using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using System.Windows.Controls;

namespace Netytar
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetytarDMIBox : NeeqDMIs.DMIBox
    {
        public Eyetracker Eyetracker { get; set; } = Eyetracker.Tobii;
        public KeyboardModule KeyboardModule;
        public MainWindow NetytarMainWindow { get; set; }

        private string testString;
        public string TestString { get => testString; set => testString = value; }

        private NetytarControlModes netytarControlMode = NetytarControlModes.Keyboard;
        public NetytarControlModes NetytarControlMode { get => netytarControlMode; set { netytarControlMode = value; ResetModulationAndPressure(); } }
        private ModulationControlModes modulationControlMode = ModulationControlModes.On;
        public ModulationControlModes ModulationControlMode { get => modulationControlMode; set { modulationControlMode = value; ResetModulationAndPressure(); } }
        private BreathControlModes breathControlMode = BreathControlModes.Dynamic;
        public BreathControlModes BreathControlMode { get => breathControlMode; set { breathControlMode = value; ResetModulationAndPressure(); } }

        private Button lastGazedButton = new Button();
        public Button LastGazedButton { get => lastGazedButton; set => lastGazedButton = value; }
        private bool hasAButtonGaze = false;
        public bool HasAButtonGaze { get => hasAButtonGaze; set => hasAButtonGaze = value; }

        #region Instrument logic
        private bool blow = false;
        private int velocity = 127;
        private int pressure = 127;
        private int modulation = 0;
        private MidiNotes selectedNote = MidiNotes.C5;

        public void ResetModulationAndPressure()
        {
            Blow = false;
            Modulation = 0;
            Pressure = 127;
            Velocity = 127;
        }

        public bool Blow
        {
            get { return blow; }
            set
            {
                if(value != blow)
                {
                    blow = value;
                    if (blow == true)
                    {
                        PlaySelectedNote();
                    }
                    else
                    {
                        StopSelectedNote();
                    }
                }   
            }
            
        }
        public int Pressure
        {
            get { return pressure; }
            set
            {
                if(BreathControlMode == BreathControlModes.Dynamic)
                {
                    if (value < 50 && value > 1)
                    {
                        pressure = 50;
                    }
                    else if (value > 127)
                    {
                        pressure = 127;
                    }
                    else if (value == 0)
                    {
                        pressure = 0;
                    }
                    else
                    {
                        pressure = value;
                    }
                    SetPressure();
                }
                if(BreathControlMode == BreathControlModes.Switch)
                {
                    pressure = 127;
                    SetPressure();
                }
            }
        }
        public int Modulation
        {
            get { return modulation; }
            set
            {
                if(ModulationControlMode == ModulationControlModes.On)
                {
                    if (value < 50 && value > 1)
                    {
                        modulation = 50;
                    }
                    else if (value > 127)
                    {
                        modulation = 127;
                    }
                    else if (value == 0)
                    {
                        modulation = 0;
                    }
                    else
                    {
                        modulation = value;
                    }
                    SetModulation();
                }
                else if (ModulationControlMode == ModulationControlModes.Off)
                {
                    modulation = 0;
                    SetModulation();
                }
            }
        }

        public int Velocity
        {
            get { return velocity; }
            set
            {
                if (value < 0)
                {
                    velocity = 0;
                }
                else if (value > 127)
                {
                    velocity = 127;
                }
                else
                {
                    velocity = value;
                }
            }
        }

        public MidiNotes SelectedNote
        {
            get { return selectedNote; }
            set
            {
                if(value != selectedNote)
                {
                    StopSelectedNote();
                    selectedNote = value;
                    if (blow)
                    {
                        PlaySelectedNote();
                    }
                }
            }
        }

        private void StopSelectedNote()
        {
            MidiModule.StopNote((int)selectedNote);
        }
        private void PlaySelectedNote()
        {
            MidiModule.PlayNote((int)selectedNote, velocity);
        }
        private void SetPressure()
        {
            MidiModule.SetPressure(pressure);
        }
        private void SetModulation()
        {
            MidiModule.SetModulation(Modulation);
        }
        #endregion

        #region Graphic components
        private AutoScroller autoScroller;
        public AutoScroller AutoScroller { get => autoScroller; set => autoScroller = value; }
        
        private NetytarSurface netytarSurface;
        public NetytarSurface NetytarSurface { get => netytarSurface; set => netytarSurface = value; }
        #endregion

        #region Extra sensors
        private SensorModule sensorReader;
        public SensorModule SensorReader { get => sensorReader; set => sensorReader = value; }
        #endregion

        #region Shared values
        private double eyePosBaseX = 0;
        private double eyePosBaseY = 0;
        private double eyePosBaseZ = 0;
        private int gyroBaseX = 0;
        private int gyroBaseY = 0;
        private int gyroBaseZ = 0;
        private int accBaseX = 0;
        private int accBaseY = 0;
        private int accBaseZ = 0;
        private int gyroX = 0;
        private int gyroY = 0;
        private int gyroZ = 0;
        private int accX = 0;
        private int accY = 0;
        private int accZ = 0;
        public double HeadPoseBaseX { get => eyePosBaseX; set => eyePosBaseX = value; }
        public double HeadPoseBaseY { get => eyePosBaseY; set => eyePosBaseY = value; }
        public double HeadPoseBaseZ { get => eyePosBaseZ; set => eyePosBaseZ = value; }
        public int GyroBaseX { get => gyroBaseX; set => gyroBaseX = value; }
        public int GyroBaseY { get => gyroBaseY; set => gyroBaseY = value; }
        public int GyroBaseZ { get => gyroBaseZ; set => gyroBaseZ = value; }
        public int AccBaseX { get => accBaseX; set => accBaseX = value; }
        public int AccBaseY { get => accBaseY; set => accBaseY = value; }
        public int AccBaseZ { get => accBaseZ; set => accBaseZ = value; }
        public int GyroX { get => gyroX; set => gyroX = value; }
        public int GyroY { get => gyroY; set => gyroY = value; }
        public int GyroZ { get => gyroZ; set => gyroZ = value; }
        public int AccX { get => accX; set => accX = value; }
        public int AccY { get => accY; set => accY = value; }
        public int AccZ { get => accZ; set => accZ = value; }
        public int GyroCalibX { get => gyroX - GyroBaseX;}
        public int GyroCalibY { get => gyroY - GyroBaseY;}
        public int GyroCalibZ { get => gyroZ - GyroBaseZ;}
        public int AccCalibX { get => accX - GyroBaseX;}
        public int AccCalibY { get => accY - GyroBaseY;}
        public int AccCalibZ { get => accZ - GyroBaseZ;}

        public void Dispose()
        {
            try
            {
                TobiiModule.Dispose();
            }
            catch
            {

            }
            try
            {
                SensorReader.Disconnect();
            }
            catch
            {

            }

        }

        public void CalibrateGyroBase()
        {
            Rack.NetytarDMIBox.gyroBaseX = gyroX;
            Rack.NetytarDMIBox.gyroBaseY = gyroY;
            Rack.NetytarDMIBox.gyroBaseZ = gyroZ;
        }

        public void CalibrateAccBase()
        {
            Rack.NetytarDMIBox.accBaseX = accX;
            Rack.NetytarDMIBox.accBaseY = accY;
            Rack.NetytarDMIBox.accBaseZ = accZ;
        }
        #endregion
    }

    public enum Eyetracker
    {
        Tobii,
        Eyetribe
    }

    public enum NetytarControlModes
    {
        Keyboard,
        BreathSensor,
        EyePos,
        EyeVel
    }

    public enum ModulationControlModes
    {
        On,
        Off
    }

    public enum BreathControlModes
    {
        Dynamic,
        Switch
    }
}
