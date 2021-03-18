using NeeqDMIs.Music;
using Netychords.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Netychords.Surface.FlowerLayout
{
    public class FlowerButton
    {
        public const int buttonDim = 10;
        public const int occluderDim = 30;
        public FlowerFamilies Family { get; private set; }

        public MidiNotes RootNote { get; private set; }

        public FlowerButton(MidiNotes rootNote, FlowerConfig config, int Xcenter, int Ycenter, NetychordsSurface surface, FlowerGridDimensions gridDim)
        {
            this.RootNote = rootNote;
            LoadFlowerConfig(config);
            CreateButtons(surface);
            SetButtonsDimensions(buttonDim, occluderDim);
            SetPositions(Xcenter, Ycenter, gridDim);
            DrawButtons(surface);
        }

        private void DrawButtons(NetychordsSurface surface)
        {
            foreach (NetychordsButton button in Buttons)
            {
                surface.Canvas.Children.Add(button);
            }
        }
        private void SetButtonsDimensions(int buttonDim, int occluderDim)
        {
            foreach(NetychordsButton button in Buttons)
            {
                button.Occluder.Width = occluderDim;
                button.Occluder.Height = occluderDim;
                button.Height = buttonDim;
                button.Width = buttonDim;
            }
        }

        private void CreateButtons(NetychordsSurface surface)
        {
            Button_C = new NetychordsButton(surface);
            Button_C.Chord = Chord_C;
            Button_C.Occluder.Fill = new SolidColorBrush(Family.GetColor(FlowerButtonPositions.C));
            Panel.SetZIndex(Button_C, 30);
            Panel.SetZIndex(Button_C.Occluder, 2);

            Button_D = new NetychordsButton(surface);
            Button_D.Chord = Chord_D;
            Button_D.Occluder.Fill = new SolidColorBrush(Family.GetColor(FlowerButtonPositions.D));
            Panel.SetZIndex(Button_D, 30);
            Panel.SetZIndex(Button_D.Occluder, 2);

            Button_U = new NetychordsButton(surface);
            Button_U.Chord = Chord_U;
            Button_U.Occluder.Fill = new SolidColorBrush(Family.GetColor(FlowerButtonPositions.U));
            Panel.SetZIndex(Button_U, 30);
            Panel.SetZIndex(Button_U.Occluder, 2);

            Button_R = new NetychordsButton(surface);
            Button_R.Chord = Chord_R;
            Button_R.Occluder.Fill = new SolidColorBrush(Family.GetColor(FlowerButtonPositions.R));
            Panel.SetZIndex(Button_R, 30);
            Panel.SetZIndex(Button_R.Occluder, 2);

            Button_L = new NetychordsButton(surface);
            Button_L.Chord = Chord_L;
            Button_L.Occluder.Fill = new SolidColorBrush(Family.GetColor(FlowerButtonPositions.L));
            Panel.SetZIndex(Button_L, 30);
            Panel.SetZIndex(Button_L.Occluder, 2);

            Buttons = new List<NetychordsButton>
            {
                Button_C,
                Button_D,
                Button_L,
                Button_U,
                Button_R
            };
        }

        private void LoadFlowerConfig(FlowerConfig config)
        {
            Family = config.Family;
            Chord_C = new MidiChord(RootNote, config.ChordType_C);
            Chord_L = new MidiChord(RootNote, config.ChordType_L);
            Chord_R = new MidiChord(RootNote, config.ChordType_R);
            Chord_U = new MidiChord(RootNote, config.ChordType_U);
            Chord_D = new MidiChord(RootNote, config.ChordType_D);
        }

        private void SetPositions(int xcenter, int ycenter, FlowerGridDimensions gridDim)
        {
            X_C = xcenter;
            X_L = xcenter - 1;
            X_R = xcenter + 1;
            X_U = xcenter;
            X_D = xcenter;

            Y_C = ycenter;
            Y_L = ycenter;
            Y_R = ycenter;
            Y_U = ycenter + 1;
            Y_D = ycenter - 1;

            Canvas.SetLeft(Button_C, X_C * gridDim.X);
            Canvas.SetLeft(Button_L, X_L * gridDim.X);
            Canvas.SetLeft(Button_R, X_R * gridDim.X);
            Canvas.SetLeft(Button_D, X_D * gridDim.X);
            Canvas.SetLeft(Button_U, X_U * gridDim.X);

            Canvas.SetTop(Button_C, Y_C * gridDim.Y);
            Canvas.SetTop(Button_L, Y_L * gridDim.Y);
            Canvas.SetTop(Button_R, Y_R * gridDim.Y);
            Canvas.SetTop(Button_D, Y_D * gridDim.Y);
            Canvas.SetTop(Button_U, Y_U * gridDim.Y);
        }

        #region Buttons

        public List<NetychordsButton> Buttons { get; private set; }
        public NetychordsButton Button_C { get; set; }
        public NetychordsButton Button_D { get; set; }
        public NetychordsButton Button_L { get; set; }
        public NetychordsButton Button_R { get; set; }
        public NetychordsButton Button_U { get; set; }

        #endregion Buttons

        #region Positions

        public int X_C { get; set; }
        public int X_D { get; set; }
        public int X_L { get; set; }
        public int X_R { get; set; }
        public int X_U { get; set; }
        public int Y_C { get; set; }
        public int Y_D { get; set; }
        public int Y_L { get; set; }
        public int Y_R { get; set; }
        public int Y_U { get; set; }

        #endregion Positions

        #region Chords

        private MidiChord Chord_C { get; set; }
        private MidiChord Chord_D { get; set; }
        private MidiChord Chord_L { get; set; }
        private MidiChord Chord_R { get; set; }
        private MidiChord Chord_U { get; set; }

        #endregion Chords
    }
}