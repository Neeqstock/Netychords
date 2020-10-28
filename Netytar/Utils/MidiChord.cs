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
        public List<int> interval;

        public MidiChord(MidiNotes root, ChordType type)
        {
            rootNote = root;
            chordType = type;
            interval = GenerateInterval(type);
        }

        public List<int> GenerateInterval(ChordType type)
        {
            List<int> temp = new List<int> ();

            temp.Add(0);

            if (type == ChordType.Major) { temp.Add(4); temp.Add(7); };
            if (type == ChordType.Minor) { temp.Add(3); temp.Add(7); };
            if (type == ChordType.MajorSeventh) { temp.Add(4); temp.Add(7); temp.Add(11); };
            if (type == ChordType.MinorSeventh) { temp.Add(3); temp.Add(7); temp.Add(10); };
            if (type == ChordType.DominantSeventh) { temp.Add(4); temp.Add(7); temp.Add(10); };
            if (type == ChordType.DiminishedSeventh) { temp.Add(3); temp.Add(6); temp.Add(9); };
            if (type == ChordType.Sus2) { temp.Add(2); temp.Add(7); };
            if (type == ChordType.Sus4) { temp.Add(5); temp.Add(7); };
            if (type == ChordType.Augmented) { temp.Add(4); temp.Add(8); };
            if (type == ChordType.DominantNinth) { temp.Add(4); temp.Add(7); temp.Add(14); };
            if (type == ChordType.DominantEleventh) { temp.Add(4); temp.Add(7); temp.Add(14); temp.Add(17); };

            return temp;
        }

        public static MidiChord ChordFactory(string note, string octaveNumber, ChordType chordType)
        {
            string midiNote;


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
            /*if (chordType == ChordType.DominantSeventh)
            {
                octaveNumber = (int.Parse(octaveNumber) - 1).ToString();
            }*/
            midiNote = midiNote + octaveNumber;

            MidiNotes rootNote = (MidiNotes)System.Enum.Parse(typeof(MidiNotes), midiNote);

            return new MidiChord(rootNote, chordType);
        }
    };

    public enum ChordType
    {
        Major,//
        Minor,//
        MajorSeventh,//
        MinorSeventh,//
        DominantSeventh,//
        DiminishedSeventh,
        Sus2,//
        Sus4,//
        Augmented,
        DominantNinth,
        DominantEleventh,
    };
}
