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
    public class NetychordsDMIBox : DMIBox
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
                    }
                    else
                    {
                        StopChord(chord);
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
                }else
                if (!keyDown && value)
                {
                    PlayChord(chord);
                    keyDown = value;
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

        #endregion
    }


}
