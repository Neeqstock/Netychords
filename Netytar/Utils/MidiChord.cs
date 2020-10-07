using NeeqDMIs.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netytar.Utils
{
    public class MidiChord
    {
        public MidiNotes rootNote;
        public ChordType chordType;

        public MidiChord(MidiNotes root, ChordType type)
        {
            rootNote = root;
            chordType = type;
        }
        public static MidiChord StringToNote(string note, int octaveNumber)
        {
            ChordType chordType;
            string midiNote;


            if (note.Contains("m"))
            {
                chordType = ChordType.Minor;
            }
            else
            {
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
    };

    public enum ChordType
    {
        Major,
        Minor,
        DominantSeventh,
        DiminishedSeventh
    };
}
