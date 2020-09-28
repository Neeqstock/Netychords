using NeeqDMIs.Eyetracking.Eyetribe;
using NeeqDMIs.Eyetracking.Tobii;
using NeeqDMIs.Keyboard;
using NeeqDMIs.MIDI;
using NeeqDMIs.Music;

namespace NeeqDMIs
{
    public abstract class DMIBox
    {
        // EYETRACKERS
        public TobiiModule TobiiModule;
        public EyeTribeModule EyeTribeModule;

        // MUSIC MODULE
        public MusicModule MusicModule;

        // MISCELLANEOUS
        public IMidiModule MidiModule;
    }
}
