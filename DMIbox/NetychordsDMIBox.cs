using EyeTribe.ClientSdk.Data;
using HANDMIs_TestSuite.Utils;
using NAudio.MediaFoundation;
using NeeqDMIs;
using NeeqDMIs.ATmega;
using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using Netytar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public KeyboardModule KeyboardModule;
        public MainWindow MainWindow { get; set; }
        /*public DateTime startcalibration = new DateTime(2020, 01, 01, 00, 00, 00);*/


       /* public bool calibrateStarted = false;
        public bool calibrateEnded = false;*/

        #region Instrument logic
        //private bool blow = false;
        private int velocity = 127;
        private int pressure = 127;
        private int modulation = 0;
        public List<int> reeds = new List<int>();

        private MidiChord chord = new MidiChord(MidiNotes.C4, ChordType.Major);
        public MidiChord lastChord;
        private bool keyDown = false;
        public string octaveNumber = "4";
        public string firstNote = "C";
        public string isPlaying = "";
        public string layout = "Fifth circle";
        public List<string> arbitraryLines = new List<string>();
        public bool strummed = false;
        public bool keyboardEmulator = true;


        public MidiChord Chord
        {
            get { return chord; }
            set
            {
                if (keyboardEmulator)
                {
                    if (!(value.chordType == chord.chordType && value.rootNote == chord.rootNote))
                    {
                        if (keyDown)
                        {
                            StopChord(chord);
                            PlayChord(value);
                            isPlaying = "Playing";
                            isEndedStrum = false;

                        }
                        else
                        {
                            StopChord(chord);
                            isPlaying = "";
                        }
                        chord = value;
                    }
                }
                else
                {
                    if (!(value.chordType == chord.chordType && value.rootNote == chord.rootNote))
                    {/*
                        if (keyDown)
                        {
                            StopChord(chord);
                            PlayChord(value);
                            isPlaying = "Playing";
                            isEndedStrum = false;

                        }
                        else
                        {
                            StopChord(chord);
                            isPlaying = "";
                        }*/
                        chord = value;
                    }
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
            /*for (int i = 0; i < chord.interval.Count; i++)
            {
                MidiModule.StopNote((int)chord.rootNote + chord.interval[i]);
            }*/

            for (int i = 12; i < 128; i++)
            {
                MidiModule.StopNote(i);
            }
        }
        public void PlayChord(MidiChord chord)
        {
            List<int> notes = new List<int>();
            int minInterval;
            int maxInterval;

            /*switch (chord.rootNote.ToStandardString().Remove(chord.rootNote.ToStandardString().Length - 1))
            {
                case "C":
                    minInterval = 36;
                    maxInterval = 47;
                    break;
                case "C#":
                    minInterval = 37;
                    maxInterval = 48;
                    break;
            }*/

            for (int i = 0; i < chord.interval.Count; i++)
            {
                int thisNote = (int)chord.rootNote + chord.interval[i];

                for (int j=0; j<5; j++)
                {
                    minInterval = 36 + j * 12;
                    maxInterval = 47 + j * 12;

                    if (reeds.Contains(j))
                    {
                        if ((thisNote + (j+1)*12 <= maxInterval && thisNote + (j+1)*12 >= minInterval))
                        {
                            if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j+1)*12)))
                            {
                                notes.Add((int)chord.rootNote + chord.interval[i] + (j+1)*12);
                            }
                        }
                        if (thisNote + j*12 <= maxInterval && thisNote + j*12 >= minInterval)
                        {
                            if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j) * 12)))
                            {
                                notes.Add((int)chord.rootNote + chord.interval[i] + j*12);
                            }
                        }
                        if (thisNote + (j-1)*12 <= maxInterval && thisNote + (j-1)*12 >= minInterval)
                        {
                            if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j - 1) * 12)))
                            {
                                notes.Add((int)chord.rootNote + chord.interval[i] + (j-1)*12);
                            }
                        }
                    }
                }
                //MidiModule.PlayNote((int)chord.rootNote + chord.interval[i], velocity);
            }

            if(!(reeds.Count == 0))
            {

                int min = reeds.Min();
                notes.Add((int)chord.rootNote + (min - 1) * 12);
            }


            for (int i = 0; i < notes.Count; i++)
            {
                MidiModule.PlayNote(notes[i], velocity);
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


        /*private CalibrationSurface calibrationSurface;
        public CalibrationSurface CalibrationSurface { get => calibrationSurface; set => calibrationSurface = value; }*/
        #endregion

        #region HeadSensor

        public SensorModule HeadTrackerModule { get; set; }

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

        /*public List<TargetDistance> TargetDistances = new List<TargetDistance>()
        {
            new TargetDistance(300),
            new TargetDistance(300),
            new TargetDistance(500),
            new TargetDistance(500),
            new TargetDistance(700),
            new TargetDistance(700),
            new TargetDistance(900),
            new TargetDistance(900),
        };*/

        public bool isCentered = false;
        public double minYaw;
        public double maxYaw;

        public void CalibrationHeadSensor()
        {

            if (/*calibrateStarted && calibrateEnded &&*/ !isCentered)
            {
                minYaw = HeadTrackerData.TranspCalibrationYaw - 8;
                maxYaw = HeadTrackerData.TranspCalibrationYaw + 8;
                isCentered = true;
            }
        }

        public double startStrum;
        public double endStrum;
        public bool isStartedStrum = false;
        public bool isEndedStrum = false;
        public DateTime startingTime;
        public DateTime endingTime;
        public directionStrum dirStrum;
        public enum directionStrum
        {
            Right,
            Left
        }
        #endregion

        #region Strumming

        public void elaborateStrumming()
        {
            double lastYaw = 0;
            if (isCentered && MainWindow.NetychordsStarted)
            {
                if (HeadTrackerData.TranspYaw <= maxYaw && HeadTrackerData.TranspYaw >= minYaw)
                {
                    startStrum = HeadTrackerData.TranspYaw;
                    isEndedStrum = false;
                }
                else if (!isStartedStrum && !isEndedStrum)
                {
                    if (HeadTrackerData.TranspYaw < minYaw)
                    {
                        dirStrum = NetychordsDMIBox.directionStrum.Left;
                        startingTime = DateTime.Now;
                        isStartedStrum = true;
                        isEndedStrum = false;
                        lastYaw = HeadTrackerData.TranspYaw;
                    }
                    else if (HeadTrackerData.TranspYaw > maxYaw)
                    {
                        dirStrum = NetychordsDMIBox.directionStrum.Right;
                        startingTime = DateTime.Now;
                        isStartedStrum = true;
                        isEndedStrum = false;
                        lastYaw = HeadTrackerData.TranspYaw;
                    }
                }
                else if (!isEndedStrum)
                {
                    switch (dirStrum)
                    {
                        case NetychordsDMIBox.directionStrum.Left:
                            if (HeadTrackerData.TranspYaw > lastYaw)
                            {
                                endStrum = lastYaw;
                                endingTime = DateTime.Now;
                                double distance = Math.Abs(endStrum - minYaw);
                                int midiVelocity = (int)(40 + 1.4 * distance);
                                isEndedStrum = true;
                                isStartedStrum = false;
                                Velocity = midiVelocity;
                                /*if (lastChord != null && lastChord != Chord)
                                {
                                    StopChord(lastChord);
                                }*/
                                //StopChord(NetychordsSurface.LastCheckedButton.Chord);

                                if (lastChord != null)
                                {
                                    StopChord(lastChord);
                                }
                                PlayChord(Chord);
                                lastChord = Chord;
                            }
                            else
                            {
                                lastYaw = HeadTrackerData.TranspYaw;

                            }
                            break;

                        case NetychordsDMIBox.directionStrum.Right:
                            if (HeadTrackerData.TranspYaw < lastYaw)
                            {
                                endStrum = lastYaw;
                                endingTime = DateTime.Now;
                                double distance = Math.Abs(endStrum - maxYaw);
                                int midiVelocity = (int)(40 + 1.4 * distance);
                                isEndedStrum = true;
                                isStartedStrum = false;
                                Velocity = midiVelocity;
                                //StopChord(NetychordsSurface.LastCheckedButton.Chord);
                                if (lastChord != null)
                                {
                                    StopChord(lastChord);
                                }
                                PlayChord(Chord);
                                lastChord = Chord;
                                /*
                                if (lastChord != null && lastChord != Chord)
                                {
                                    StopChord(lastChord);
                                }*/

                            }

                            else
                            {
                                lastYaw = HeadTrackerData.TranspYaw;

                            }
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
    