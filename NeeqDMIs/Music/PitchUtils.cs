using System;

namespace NeeqDMIs.Music
{
    public static class PitchUtils
    {
        /// <summary>
        /// Converts a pitch number (int) to the correspondent MidiNote.
        /// </summary>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static MidiNotes PitchToMidiNote(int pitch)
        {
            foreach (MidiNotes note in Enum.GetValues(typeof(MidiNotes)))
            {
                if ((int)note == pitch)
                {
                    return note;
                }
            }
            return MidiNotes.NaN;
        }

        /// <summary>
        /// Converts a pitch number (int) to the correspondent AbsNote.
        /// </summary>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static AbsNotes PitchToAbsNote(int pitch)
        {
            MidiNotes midiNote = PitchToMidiNote(pitch);
            return midiNote.ToAbsNote();
        }
    }
}
