using System.Collections.Generic;

namespace NeeqDMIs.Music
{
    public class MusicModule
    {
        private List<MidiNotes> playingNotes = new List<MidiNotes>();

        public List<MidiNotes> PlayingNotes { get => playingNotes; set => playingNotes = value; }
    }
}