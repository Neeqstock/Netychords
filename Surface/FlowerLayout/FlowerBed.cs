using NeeqDMIs.Music;
using System.Collections.Generic;
using System.Drawing;

namespace Netychords.Surface.FlowerLayout
{
    public class FlowerBed
    {
        public readonly List<int> MajorRule = new List<int> { 1, 3, 5, 6, 8, 10, 12 };
        public readonly List<int> MinorRule = new List<int> { 1, 3, 4, 6, 8, 9, 11 };

        private  Point pF1 = new Point(0, 0);
        private  Point pF2 = new Point(-3, -1);
        private  Point pF3 = new Point(-2, 1);
        private  Point pF4 = new Point(1, 2);
        private  Point pF5 = new Point(3, 1);
        private  Point pF6 = new Point(2, -1);
        private  Point pF7 = new Point(-1, -2);

        public FlowerButton Flower_1 { get; set; }
        public FlowerButton Flower_2 { get; set; }
        public FlowerButton Flower_3 { get; set; }
        public FlowerButton Flower_4 { get; set; }
        public FlowerButton Flower_5 { get; set; }
        public FlowerButton Flower_6 { get; set; }
        public FlowerButton Flower_7 { get; set; }

        public FlowerBed(MidiNotes rootNote, FlowerFamilies rootFamily, FlowerGridDimensions gridDim, int Xcenter, int Ycenter, NetychordsButton[,] netychordsButtons)
        {
            List<MidiNotes> allnotes = MidiNotesUtils.GetAllMidiNotesList();
            List<int> rule = new List<int>();

            pF1.X += Xcenter; pF1.Y += Ycenter;
            pF2.X += Xcenter; pF2.Y += Ycenter;
            pF3.X += Xcenter; pF3.Y += Ycenter;
            pF4.X += Xcenter; pF4.Y += Ycenter;
            pF5.X += Xcenter; pF5.Y += Ycenter;
            pF6.X += Xcenter; pF6.Y += Ycenter;
            pF7.X += Xcenter; pF7.Y += Ycenter;

            switch (rootFamily)
            {
                case FlowerFamilies.Major:

                    rule = MajorRule;

                    MidiNotes n1 = rootNote;
                    MidiNotes n2 = (MidiNotes)rootNote + rule[1];
                    MidiNotes n3 = (MidiNotes)rootNote + rule[2];
                    MidiNotes n4 = (MidiNotes)rootNote + rule[3];
                    MidiNotes n5 = (MidiNotes)rootNote + rule[4];
                    MidiNotes n6 = (MidiNotes)rootNote + rule[5];
                    MidiNotes n7 = (MidiNotes)rootNote + rule[6];

                    Flower_1 = new FlowerButton(n1, FlowerConfigFactory.DefaultMajor(), pF1.X, pF1.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);
                    Flower_2 = new FlowerButton(n2, FlowerConfigFactory.DefaultMinor(), pF2.X, pF2.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);
                    Flower_3 = new FlowerButton(n3, FlowerConfigFactory.DefaultMinor(), pF3.X, pF3.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);
                    Flower_4 = new FlowerButton(n4, FlowerConfigFactory.DefaultMajor(), pF4.X, pF4.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);
                    Flower_5 = new FlowerButton(n5, FlowerConfigFactory.DefaultMajor(), pF5.X, pF5.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);
                    Flower_6 = new FlowerButton(n6, FlowerConfigFactory.DefaultMinor(), pF6.X, pF6.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);
                    Flower_7 = new FlowerButton(n7, FlowerConfigFactory.DefaultMinor(), pF7.X, pF7.Y, Rack.NetychordsDMIBox.NetychordsSurface, gridDim);

                    break;

                case FlowerFamilies.Minor:
                    rule = MinorRule;
                    break;
            }
        }
    }
}