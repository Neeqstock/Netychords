using Netychords.Utils;

namespace Netychords.Surface.FlowerLayout
{
    public class FlowerConfig
    {
        public FlowerFamilies Family { get; set; }
        public ChordType Type_D { get; set; }
        public ChordType Type_U { get; set; }
        public ChordType Type_L { get; set; }
        public ChordType Type_R { get; set; }
        public ChordType Type_C { get; set; }
    }

    public static class FlowerConfigFactory
    {
        public static FlowerConfig DefaultMajor()
        {
            return new FlowerConfig
            {
                Family = FlowerFamilies.Major,
                Type_C = ChordType.Major,
                Type_L = ChordType.MajorSeventh,
                Type_U = ChordType.MajorSixth,
                Type_R = ChordType.DominantSeventh,
                Type_D = ChordType.Sus2
            };
        }
        public static FlowerConfig DefaultMinor()
        {
            return new FlowerConfig
            {
                Family = FlowerFamilies.Minor,
                Type_C = ChordType.Minor,
                Type_L = ChordType.MinorSeventh,
                Type_U = ChordType.MinorSixth,
                Type_R = ChordType.DiminishedSeventh,
                Type_D = ChordType.SemiDiminished
            };
        }
    }
    public class FlowerButton
    {
        public FlowerFamilies Family { get; set; }
        public NetychordsButton Button_C { get; set; }
        public NetychordsButton Button_D { get; set; }
        public NetychordsButton Button_L { get; set; }
        public NetychordsButton Button_R { get; set; }
        public NetychordsButton Button_U { get; set; }

        public MidiChord RootChord
        { get; set; }

        public int X_C { get; set; }
        public int Y_C { get; set; }
        public int X_L { get; set; }
        public int Y_L { get; set; }
        public int X_R { get; set; }
        public int Y_R { get; set; }
        public int X_U { get; set; }
        public int Y_U { get; set; }
        public int X_D { get; set; }
        public int Y_D { get; set; }
        private MidiChord Chord_D { get; set; }
        private MidiChord Chord_L { get; set; }
        private MidiChord Chord_R { get; set; }
        private MidiChord Chord_U { get; set; }
        private MidiChord Chord_C { get; set; }
    }
}