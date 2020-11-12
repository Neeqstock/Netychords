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
            List<int> temp = new List<int>();

            if (Rack.NetychordsDMIBox != null)
            {
                /*for (int i = 0; i < Rack.NetychordsDMIBox.reeds.Count; i++)
                {
                    temp.Add(Rack.NetychordsDMIBox.reeds[i]*12);

                    if (type == ChordType.Major) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 4); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); };
                    if (type == ChordType.Minor) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 3); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); };
                    if (type == ChordType.MajorSeventh) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 4); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 11); };
                    if (type == ChordType.MinorSeventh) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 3); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 10); };
                    if (type == ChordType.DominantSeventh) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 4); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 10); };
                    if (type == ChordType.DiminishedSeventh) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 3); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 6); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 9); };
                    if (type == ChordType.Sus2) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 2); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); };
                    if (type == ChordType.Sus4) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 5); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); };
                    if (type == ChordType.Augmented) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 4); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 8); };
                    if (type == ChordType.DominantNinth) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 4); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 14); };
                    if (type == ChordType.DominantEleventh) { temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 4); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 7); temp.Add(i + 14); temp.Add(Rack.NetychordsDMIBox.reeds[i] * 12 + 17); };
                }*/

                //if (Rack.NetychordsDMIBox.reeds.Count == 0)
                //{
                //}
            }



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

        public string ChordName()
        {
            string name = rootNote.ToStandardString().Remove(rootNote.ToStandardString().Length - 1) + ChordTypeAbbreviation();
            return name;
        }

        public string ChordTypeAbbreviation()
        {
            ChordType type = chordType;
            string name;
            switch (type)
            {
                case ChordType.Major:
                    name = "";
                    break;
                case ChordType.Minor:
                    name = "m";
                    break;
                case ChordType.Augmented:
                    name = "aug";
                    break;
                case ChordType.DiminishedSeventh:
                    name = "dim7";
                    break;
                case ChordType.DominantEleventh:
                    name = "11";
                    break;
                case ChordType.DominantNinth:
                    name = "9";
                    break;
                case ChordType.DominantSeventh:
                    name = "7";
                    break;
                case ChordType.MajorSeventh:
                    name = "maj7";
                    break;
                case ChordType.MinorSeventh:
                    name = "min7";
                    break;
                case ChordType.Sus2:
                    name = "sus2";
                    break;
                case ChordType.Sus4:
                    name = "sus4";
                    break;
                default:
                    name = "";
                    break;
            }

            return name;
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
                        midiNote += "G";
                        break;
                    case 'B':
                        midiNote += "A";
                        break;
                    case 'D':
                        midiNote += "C";
                        break;
                    case 'E':
                        midiNote += "D";
                        break;
                    case 'G':
                        midiNote += "F";
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
            midiNote += octaveNumber;

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
