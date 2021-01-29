using NeeqDMIs.ATmega;
using NeeqDMIs.Headtracking.NeeqHT;
using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using Netytar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Netytar
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetychordsDMIBox : NeeqDMIs.DMIBox
    {
        public KeyboardModule KeyboardModule;
        public Eyetracker Eyetracker { get; set; } = Eyetracker.Tobii;
        public MainWindow MainWindow { get; set; }
        /*public DateTime startcalibration = new DateTime(2020, 01, 01, 00, 00, 00);*/

        /* public bool calibrateStarted = false;
         public bool calibrateEnded = false;*/

        #region Instrument logic

        public List<string> arbitraryLines = new List<string>();

        public string firstNote = "C";

        public string isPlaying = "";

        public bool keyboardEmulator = true;

        public MidiChord lastChord;

        public string layout = "Fifth circle";

        public string octaveNumber = "4";

        public List<int> reeds = new List<int>();

        public bool strummed = false;

        private MidiChord chord = new MidiChord(MidiNotes.C4, ChordType.Major);

        private bool keyDown = false;

        private int modulation = 0;

        private int pressure = 127;

        //private bool blow = false;
        private int velocity = 127;

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

        public int Modulation
        {
            get { return modulation; }
            set
            {
                modulation = 0;
                SetModulation();
            }
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

                for (int j = 0; j < 5; j++)
                {
                    minInterval = 36 + j * 12;
                    maxInterval = 47 + j * 12;

                    if (reeds.Contains(j))
                    {
                        if ((thisNote + (j + 1) * 12 <= maxInterval && thisNote + (j + 1) * 12 >= minInterval))
                        {
                            if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j + 1) * 12)))
                            {
                                notes.Add((int)chord.rootNote + chord.interval[i] + (j + 1) * 12);
                            }
                        }
                        if (thisNote + j * 12 <= maxInterval && thisNote + j * 12 >= minInterval)
                        {
                            if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j) * 12)))
                            {
                                notes.Add((int)chord.rootNote + chord.interval[i] + j * 12);
                            }
                        }
                        if (thisNote + (j - 1) * 12 <= maxInterval && thisNote + (j - 1) * 12 >= minInterval)
                        {
                            if (!(notes.Contains((int)chord.rootNote + chord.interval[i] + (j - 1) * 12)))
                            {
                                notes.Add((int)chord.rootNote + chord.interval[i] + (j - 1) * 12);
                            }
                        }
                    }
                }
                //MidiModule.PlayNote((int)chord.rootNote + chord.interval[i], velocity);
            }

            if (!(reeds.Count == 0))
            {
                int min = reeds.Min();
                notes.Add((int)chord.rootNote + (min - 1) * 12);
            }

            for (int i = 0; i < notes.Count; i++)
            {
                MidiModule.PlayNote(notes[i], velocity);
            }
        }

        public void ResetModulationAndPressure()
        {
            Modulation = 0;
            Pressure = 127;
            Velocity = 127;
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

        private void SetModulation()
        {
            MidiModule.SetModulation(Modulation);
        }

        private void SetPressure()
        {
            MidiModule.SetPressure(pressure);
        }

        #endregion Instrument logic

        #region Graphic components

        private AutoScroller autoScroller;
        private NetychordsSurface netychordsSurface;
        public AutoScroller AutoScroller { get => autoScroller; set => autoScroller = value; }
        public NetychordsSurface NetychordsSurface { get => netychordsSurface; set => netychordsSurface = value; }

        /*private CalibrationSurface calibrationSurface;
        public CalibrationSurface CalibrationSurface { get => calibrationSurface; set => calibrationSurface = value; }*/

        #endregion Graphic components

        #region HeadSensor

        public bool isCentered = false;
        public double maxYaw;
        public double minYaw;
        private DirectionStrum dirStrum;
        private DateTime endingTime;

        private double endStrum;
        private int headTrackerPortNumber = 0;
        private bool isEndedStrum = false;
        private bool isStartedStrum = false;
        private DateTime startingTime;
        private double startStrum;
        public double CenterZone { get; set; } = 0;
        public double Distance { get; private set; }
        public HeadTrackerData HeadTrackerData { get; set; } = new HeadTrackerData();
        public SensorModule HeadTrackerModule { get; set; }

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

        public bool InDeadZone { get; set; }
        public string Str_HeadTrackerCalib { get; set; } = "Test";
        public string Str_HeadTrackerRaw { get; set; } = "Test";

        public void CalibrationHeadSensor()
        {
            if (!isCentered)
            {
                minYaw = HeadTrackerData.TranspCalibrationYaw - CenterZone;
                maxYaw = HeadTrackerData.TranspCalibrationYaw + CenterZone;
                isCentered = true;
            }
        }

        public enum DirectionStrum
        {
            Right,
            Left
        }

        #endregion HeadSensor

        #region Strumming

        public void ElaborateStrumming()
        {
            double lastYaw = 0;
            if (isCentered && MainWindow.NetychordsStarted)
            {
                if (HeadTrackerData.TranspYaw <= maxYaw && HeadTrackerData.TranspYaw >= minYaw)
                {
                    startStrum = HeadTrackerData.TranspYaw;
                    isEndedStrum = false;
                    InDeadZone = true;
                }
                else if (!isStartedStrum && !isEndedStrum)
                {
                    InDeadZone = false;
                    if (HeadTrackerData.TranspYaw < minYaw)
                    {
                        dirStrum = NetychordsDMIBox.DirectionStrum.Left;
                        startingTime = DateTime.Now;
                        isStartedStrum = true;
                        isEndedStrum = false;
                        lastYaw = HeadTrackerData.TranspYaw;
                    }
                    else if (HeadTrackerData.TranspYaw > maxYaw)
                    {
                        dirStrum = NetychordsDMIBox.DirectionStrum.Right;
                        startingTime = DateTime.Now;
                        isStartedStrum = true;
                        isEndedStrum = false;
                        lastYaw = HeadTrackerData.TranspYaw;
                    }
                }
                else if (!isEndedStrum)
                {
                    InDeadZone = false;
                    switch (dirStrum)
                    {
                        case NetychordsDMIBox.DirectionStrum.Left:
                            if (HeadTrackerData.TranspYaw > lastYaw)
                            {
                                endStrum = lastYaw;
                                endingTime = DateTime.Now;
                                Distance = endStrum - minYaw;
                                int midiVelocity = (int)(40 + 1.4 * Math.Abs(Distance));
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

                        case NetychordsDMIBox.DirectionStrum.Right:
                            if (HeadTrackerData.TranspYaw < lastYaw)
                            {
                                endStrum = lastYaw;
                                endingTime = DateTime.Now;
                                Distance = endStrum - maxYaw;
                                int midiVelocity = (int)(40 + 1.4 * Math.Abs(Distance));
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

        #endregion Strumming
    }
}