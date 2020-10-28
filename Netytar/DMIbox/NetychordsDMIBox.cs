using EyeTribe.ClientSdk.Data;
using HANDMIs_TestSuite.Utils;
using NAudio.MediaFoundation;
using NeeqDMIs;
using NeeqDMIs.ATmega;
using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using Netytar.Utils;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Netytar
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetychordsDMIBox : NeeqDMIs.DMIBox
    {
        public Eyetracker Eyetracker { get; set; } = Eyetracker.Tobii;
        public KeyboardModuleWPF KeyboardModule;
        public MainWindow MainWindow { get; set; }
        
        #region Instrument logic
        //private bool blow = false;
        private int velocity = 127;
        private int pressure = 127;
        private int modulation = 0;

        private MidiChord chord = new MidiChord(MidiNotes.C4, ChordType.Major);
        private bool keyDown = false;
        public string octaveNumber = "4";
        public string firstNote = "C";
        public string isPlaying = "";
        public string layout = "Fifth circle";
        public List<string> arbitraryLines = new List<string>();


        public MidiChord Chord
        {
            get { return chord; }
            set
            {
                if (!(value.chordType == chord.chordType && value.rootNote == chord.rootNote))
                {
                    if (keyDown)
                    {
                        StopChord(chord);
                        PlayChord(value);
                        isPlaying = "Playing";

                    }
                    else
                    {
                        StopChord(chord);
                        isPlaying = "";
                    }
                    chord = value;
                }
            }
        }

        public bool KeyDown
        {
            get { return keyDown; }
            set
            {
                if (keyDown && !value)
                {
                    StopChord(chord);
                    keyDown = value;
                    isPlaying = "";
                }
                else
                if (!keyDown && value)
                {
                    PlayChord(chord);
                    keyDown = value;
                    isPlaying = "Playing";
                }
            }
        }

        public void ResetModulationAndPressure()
        {
            Modulation = 0;
            Pressure = 127;
            Velocity = 127;
        }

        
        public int Pressure
        {
            get { return pressure; }
            set
            {
                pressure = 127;
                SetPressure();
                
            }
        }
        public int Modulation
        {
            get { return modulation; }
            set
            {
                modulation = 0;
                SetModulation();
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

        public void StopChord(MidiChord chord)
        {
            for (int i = 0; i < chord.interval.Count; i++)
            {
                MidiModule.StopNote((int)chord.rootNote + chord.interval[i]);
            }
        }
        public void PlayChord(MidiChord chord)
        {
            for (int i = 0; i < chord.interval.Count; i++)
            {
                MidiModule.PlayNote((int)chord.rootNote + chord.interval[i], velocity);
            }
            
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

        private NetychordsSurface netychordsSurface;
        public NetychordsSurface NetychordsSurface { get => netychordsSurface; set => netychordsSurface = value; }
        #endregion

        #region HeadSensor

        #region Extra sensors
        public SensorModule HeadTrackerModule { get; set; }
        #endregion       

        public string Str_HeadTrackerRaw { get; set; } = "Test";
        public string Str_HeadTrackerCalib { get; set; } = "Test";
        public HeadTrackerData HeadTrackerData { get; set; } = new HeadTrackerData();

        private int headTrackerPortNumber = 0;
        public int HeadTrackerPortNumber
        {
            get
            {
                return headTrackerPortNumber;
            }
            set
            {
                if (value < 0)
                {
                    headTrackerPortNumber = 0;
                }
                else
                {
                    headTrackerPortNumber = value;
                }
            }
        }
        public ControlDirection Direction { get; private set; }
        public Canvas XCanvas { get; set; }
        public int XCursorCenter { get; set; }

        public long XCursorValue
        {
            set
            {
                //if(value < 0)
                //{
                //   XCursorCenter = (int)(XBarLeft);
                //}
                //else if(value > GlobalValues.BarWidth)
                //{
                //    XCursorCenter = (int)(XBarLeft + GlobalValues.BarWidth);
                //}
                //else
                //{
                XCursorCenter = (int)(XBarLeft + value);
                //}

            }
        }

        public double XBarLeft { get; set; }
        public double XBarRight { get; set; }

        private ControlModes controlMode;
        public ControlModes ControlMode
        {
            get { return controlMode; }
            set
            {
                Direction = value.GetDirection();
                controlMode = value;
            }
        }
        public bool TestStarted { get; set; } = false;
        public double ScreenWidth { get; set; } = 1920;
        public double ScreenHeight { get; set; } = 1080;
        public GazeData EyeTribeGPData { get; set; }
        public int DistancesIndex { get; set; } = 0;
        public int PitchInverter { get; set; } = 1;
        public int RollInverter { get; set; } = 1;
        public bool InvertPitchRoll { get; set; } = false;

        public List<TargetDistance> TargetDistances = new List<TargetDistance>()
        {
            new TargetDistance(300),
            new TargetDistance(300),
            new TargetDistance(500),
            new TargetDistance(500),
            new TargetDistance(700),
            new TargetDistance(700),
            new TargetDistance(900),
            new TargetDistance(900),
        };

        #endregion
    }


}
