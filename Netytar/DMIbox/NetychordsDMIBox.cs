using NeeqDMIs;
using NeeqDMIs.ATmega;
using NeeqDMIs.Keyboard;
using NeeqDMIs.Music;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Netytar
{
    /// <summary>
    /// DMIBox for Netytar, implementing the internal logic of the instrument.
    /// </summary>
    public class NetychordsDMIBox : DMIBox
    {
        public enum ChordType
        {
            Major,
            Minor,
            DominantSeventh,
            DiminishedSeventh
        };
        public Eyetracker Eyetracker { get; set; } = Eyetracker.Tobii;
        public KeyboardModuleWPF KeyboardModule;
        public MainWindow MainWindow { get; set; }

        #region Instrument logic
        //private bool blow = false;
        private int velocity = 127;
        private int pressure = 127;
        private int modulation = 0;

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

        public bool playing = false;
        public MidiChord lastChord;

        public enum Notes { A, B, C, D, E, F, G};

        public class MidiChord
        {
            public MidiNotes rootNote;
            public ChordType chordType;

            public MidiChord(MidiNotes root, ChordType type)
            {
                rootNote = root;
                chordType = type;
            }
        };

        public static MidiChord StringToNote (string note, int octaveNumber)
        {
            ChordType chordType;
            string midiNote;

                        
            if (note.Contains("m"))
            {
                chordType = ChordType.Minor;
            }
            else {
                if (note.Contains("7"))
                {
                    chordType = ChordType.DominantSeventh;
                }
                else
                {
                    chordType = ChordType.Major;
                }
            };

            if (note.Contains("#"))
            {
                midiNote = "s" + note[0];
            }
            else if (note.Contains("b"))
            {
                midiNote = "s";
                char oldnote = note[0];
                switch (oldnote)
                {
                    case 'A':
                        midiNote = midiNote + "G";
                        break;
                    case 'B':
                        midiNote = midiNote + "A";
                        break;
                    case 'D':
                        midiNote = midiNote + "C";
                        break;
                    case 'E':
                        midiNote = midiNote + "D";
                        break;
                    case 'G':
                        midiNote = midiNote + "F";
                        break;
                }
            }
            else 
            {
                midiNote = "" + note[0];
            };
            if (chordType == ChordType.DominantSeventh)
            {
                octaveNumber = 3;
            }
            midiNote = midiNote + octaveNumber.ToString();

            MidiNotes rootNote = (MidiNotes)System.Enum.Parse(typeof(MidiNotes), midiNote);

            return new MidiChord(rootNote, chordType);
        }

        public void StopSelectedChord(MidiChord chord)
        {
            if (chord.chordType == ChordType.Major)
            {
                MidiModule.StopNote((int)chord.rootNote);
                MidiModule.StopNote((int)chord.rootNote + 4);
                MidiModule.StopNote((int)chord.rootNote + 7);
            }
            else if (chord.chordType == ChordType.Minor)
            {
                MidiModule.StopNote((int)chord.rootNote);
                MidiModule.StopNote((int)chord.rootNote + 3);
                MidiModule.StopNote((int)chord.rootNote + 7);
            }
            else if (chord.chordType == ChordType.DominantSeventh)
            {
                MidiModule.StopNote((int)chord.rootNote);
                MidiModule.StopNote((int)chord.rootNote + 4);
                MidiModule.StopNote((int)chord.rootNote + 7);
                MidiModule.StopNote((int)chord.rootNote + 10);
            };
            playing = false;
        }
        public void PlaySelectedChord(MidiChord chord)
        {
            if (chord.chordType == ChordType.Major)
            {
                MidiModule.PlayNote((int)chord.rootNote, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 4, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 7, velocity);
            }
            else if (chord.chordType == ChordType.Minor)
            {
                MidiModule.PlayNote((int)chord.rootNote, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 3, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 7, velocity);
            }
            else if (chord.chordType == ChordType.DominantSeventh)
            {
                MidiModule.PlayNote((int)chord.rootNote, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 4, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 7, velocity);
                MidiModule.PlayNote((int)chord.rootNote + 10, velocity);
            };
            
            playing = true;
            lastChord = chord;
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
    }


}
