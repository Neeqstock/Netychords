using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.Music
{
    public class Scale
    {
        #region Fields
        private ScaleCodes scaleCode;
        private AbsNotes rootNote;
        private List<AbsNotes> notesInScale;
        #endregion

        #region Props
        public ScaleCodes ScaleCode { get => scaleCode; set => scaleCode = value; }
        public AbsNotes RootNote { get => rootNote; set => rootNote = value; }
        public List<AbsNotes> NotesInScale { get => notesInScale; set => notesInScale = value; }
        #endregion

        private static readonly List<AbsNotes> _referenceNotes = new List<AbsNotes>()
        {
            AbsNotes.C, AbsNotes.sC, AbsNotes.D, AbsNotes.sD, AbsNotes.E, AbsNotes.F, AbsNotes.sF, AbsNotes.G, AbsNotes.sG, AbsNotes.A, AbsNotes.sA, AbsNotes.B,
            AbsNotes.C, AbsNotes.sC, AbsNotes.D, AbsNotes.sD, AbsNotes.E, AbsNotes.F, AbsNotes.sF, AbsNotes.G, AbsNotes.sG, AbsNotes.A, AbsNotes.sA, AbsNotes.B,
            AbsNotes.C, AbsNotes.sC, AbsNotes.D, AbsNotes.sD, AbsNotes.E, AbsNotes.F, AbsNotes.sF, AbsNotes.G, AbsNotes.sG, AbsNotes.A, AbsNotes.sA, AbsNotes.B
        };

        public Scale(AbsNotes rootNote, ScaleCodes scaleCode)
        {
            this.scaleCode = scaleCode;
            this.rootNote = rootNote;
            NotesInScale = GenerateNotesList(rootNote, scaleCode);
        }

        /// <summary>
        /// Generates a scale starting from the root note and the scale code.
        /// </summary>
        /// <param name="rootNote"></param>
        /// <param name="scaleCode"></param>
        /// <returns>A list of strings representing absolute note values.</returns>
        private List<AbsNotes> GenerateNotesList(AbsNotes rootNote, ScaleCodes scaleCode)
        {
            List<AbsNotes> resultScale = new List<AbsNotes>();
            int startingPos = 0;

            List<int> scaleRule = scaleCode.GetRule();

            // Determining starting pos
            for (int i = 0; i < _referenceNotes.Count; i++)
            {
                if (rootNote == _referenceNotes[i])
                {
                    startingPos = i;
                    break;
                }
            }

            // Adding elements to the returnable scale
            foreach (int pos in scaleRule)
            {
                resultScale.Add(_referenceNotes[startingPos + pos]);
            }

            return resultScale;
        }

        /// <summary>
        /// Returns the name of the scale in string format (root note + scale code).
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return rootNote.ToString() + scaleCode.ToString();
        }

        /// <summary>
        /// Returns if the pitch corresponds to a note in the scale.
        /// </summary>
        /// <param name="midiPitch"></param>
        /// <returns></returns>
        public bool IsInScale(int midiPitch)
        {
            return IsInScale(PitchUtils.PitchToAbsNote(midiPitch));
        }

        /// <summary>
        /// Returns if the note is part of the scale.
        /// </summary>
        /// <param name="absNote"></param>
        /// <returns></returns>
        public bool IsInScale(AbsNotes absNote)
        {
            return NotesInScale.Contains(absNote);
        }

        /// <summary>
        /// Returns if the midi note is in scale.
        /// </summary>
        /// <param name="midiNote"></param>
        /// <returns></returns>
        public bool IsInScale(MidiNotes midiNote)
        {
            return IsInScale(midiNote.ToAbsNote());
        }

        /// <summary>
        /// Checks if these notes are consequent in the scale (one is next to the other, in both directions). If even one of the notes is not part of the scale, returns false.
        /// </summary>
        /// <param name="note1"></param>
        /// <param name="note2"></param>
        /// <returns></returns>
        public bool AreConsequent(MidiNotes note1, MidiNotes note2)
        {
            return AreConsequent(note1.ToAbsNote(), note2.ToAbsNote());
        }

        /// <summary>
        /// Checks if these notes are consequent in the scale (one is next to the other, in both directions). If even one of the notes is not part of the scale, returns false.
        /// </summary>
        /// <param name="note1"></param>
        /// <param name="note2"></param>
        /// <returns></returns>
        public bool AreConsequent(AbsNotes note1, AbsNotes note2)
        {
            for(int i = 1; i < notesInScale.Count - 1; i++)
            {
                if (notesInScale[i].Equals(note1))
                {
                    if(notesInScale[i + 1].Equals(note2) || notesInScale[i - 1].Equals(note2))
                    {
                        return true;
                    }
                }
                if (notesInScale[i].Equals(note2))
                {
                    if (notesInScale[i + 1].Equals(note1) || notesInScale[i - 1].Equals(note1))
                    {
                        return true;
                    }
                }
            }

            if (note1.Equals(notesInScale[notesInScale.Count - 1]) && note2.Equals(notesInScale[0]))
            {
                return true;
            }

            if (note2.Equals(notesInScale[notesInScale.Count - 1]) && note1.Equals(notesInScale[0]))
            {
                return true;
            }

            return false;
        }

        public static Scale FromString(string name)
        {
            AbsNotes rootNote = AbsNotes.A;
            ScaleCodes code = ScaleCodes.chrom;
            bool rootNoteOK = false;
            bool codeOK = false;

            foreach(ScaleCodes scaleCode in Enum.GetValues(typeof(ScaleCodes)))
            {
                if (name.EndsWith(scaleCode.ToString()))
                {
                    code = scaleCode;
                    codeOK = true;
                }
            }
            foreach(AbsNotes note in Enum.GetValues(typeof(AbsNotes)))
            {
                if (name.StartsWith(note.ToString()))
                {
                    rootNote = note;
                    rootNoteOK = true;
                }
            }

            if(rootNoteOK && codeOK)
            {
                return new Scale(rootNote, code);
            }
            else
            {
                throw new Exception("The Scale class tried to parse an unexisting scale!");
            }
        }
    }
}
