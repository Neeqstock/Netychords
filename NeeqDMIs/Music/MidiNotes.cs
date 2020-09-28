using System;
using System.Linq;

namespace NeeqDMIs.Music
{
    /// <summary>
    /// Contains midi notes (Note + Octave) related to their pitch value (int). Supported range is from C0=12 to B9=119.
    /// </summary>
    public enum MidiNotes : int
    {
        NaN = 0,

        C0 = 12,
        sC0 = 13,
        D0 = 14,
        sD0 = 15,
        E0 = 16,
        F0 = 17,
        sF0 = 18,
        G0 = 19,
        sG0 = 20,
        A0 = 21,
        sA0 = 22,
        B0 = 23,
        C1 = 24,
        sC1 = 25,
        D1 = 26,
        sD1 = 27,
        E1 = 28,
        F1 = 29,
        sF1 = 30,
        G1 = 31,
        sG1 = 32,
        A1 = 33,
        sA1 = 34,
        B1 = 35,
        C2 = 36,
        sC2 = 37,
        D2 = 38,
        sD2 = 39,
        E2 = 40,
        F2 = 41,
        sF2 = 42,
        G2 = 43,
        sG2 = 44,
        A2 = 45,
        sA2 = 46,
        B2 = 47,
        C3 = 48,
        sC3 = 49,
        D3 = 50,
        sD3 = 51,
        E3 = 52,
        F3 = 53,
        sF3 = 54,
        G3 = 55,
        sG3 = 56,
        A3 = 57,
        sA3 = 58,
        B3 = 59,
        C4 = 60,
        sC4 = 61,
        D4 = 62,
        sD4 = 63,
        E4 = 64,
        F4 = 65,
        sF4 = 66,
        G4 = 67,
        sG4 = 68,
        A4 = 69,
        sA4 = 70,
        B4 = 71,
        C5 = 72,
        sC5 = 73,
        D5 = 74,
        sD5 = 75,
        E5 = 76,
        F5 = 77,
        sF5 = 78,
        G5 = 79,
        sG5 = 80,
        A5 = 81,
        sA5 = 82,
        B5 = 83,
        C6 = 84,
        sC6 = 85,
        D6 = 86,
        sD6 = 87,
        E6 = 88,
        F6 = 89,
        sF6 = 90,
        G6 = 91,
        sG6 = 92,
        A6 = 93,
        sA6 = 94,
        B6 = 95,
        C7 = 96,
        sC7 = 97,
        D7 = 98,
        sD7 = 99,
        E7 = 100,
        F7 = 101,
        sF7 = 102,
        G7 = 103,
        sG7 = 104,
        A7 = 105,
        sA7 = 106,
        B7 = 107,
        C8 = 108,
        sC8 = 109,
        D9 = 110,
        sD9 = 111,
        E9 = 112,
        F9 = 113,
        sF9 = 114,
        G9 = 115,
        sG9 = 116,
        A9 = 117,
        sA9 = 118,
        B9 = 119
    }

    public static class MidiNotesMethods
    {
        /// <summary>
        /// Returns the pitch value.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static int ToPitchValue(this MidiNotes note)
        {
            return (int)note;
        }

        /// <summary>
        /// Converts a note into the note name in standard notation, without "s" and octave number.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string ToStandardString(this MidiNotes note)
        {
            string temp = note.ToString();
            char octaveNumber = temp[temp.Length - 1];
            temp = temp.Remove(temp.Length - 1);

            if (temp.Contains('s'))
            {
                temp = temp.Remove(0, 1);
                temp = temp + "#";
            }

            return temp + octaveNumber;
        }

        /// <summary>
        /// Removes the octave notation from the note, returning the absolute note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public static AbsNotes ToAbsNote(this MidiNotes note)
        {
            string absString = note.ToString().Remove(note.ToString().Length - 1);
            return (AbsNotes)Enum.Parse(typeof(AbsNotes), absString);
        }
    }
}
