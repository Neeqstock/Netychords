using NAudio.Midi;
using System;
using System.Collections;

namespace NeeqDMIs.MIDI
{
    /// <summary>
    /// A MIDI controller module based on the library NAudio.
    /// </summary>
    public class MidiModuleNAudio : IMidiModule
    {
        private int outDevice = 0;
        private int midiChannel = 0;
        bool midiOk = false;

        private MidiOut midiOut;

        public int OutDevice { get => outDevice; set { outDevice = value; ResetMidiOut(); } }
        public int MidiChannel { get => midiChannel; set { midiChannel = value; ResetMidiOut(); } }

        public MidiModuleNAudio(int outDevice, int midiChannel)
        {
            this.outDevice = outDevice;
            this.midiChannel = midiChannel;

            ResetMidiOut();
        }

        private void ResetMidiOut()
        {
            try
            {
                midiOut = new MidiOut(this.OutDevice);
                midiOk = true;
            }
            catch
            {
                midiOk = false;
            }
        }

        public bool IsMidiOk()
        {
            return (midiOk);
        }

        public void PlayNote(int pitch, int velocity)
        {
            midiOut.Send(MidiMessage.StartNote(pitch, velocity, MidiChannel).RawData);
        }
        public void StopNote(int pitch)
        {
            midiOut.Send(MidiMessage.StopNote(pitch, 0, MidiChannel).RawData);
        }

        public void SetPressure(int pressure)
        {
            midiOut.Send(MidiMessage.ChangeControl(7, pressure, midiChannel).RawData);
        }

        public void SetModulation(int modulation)
        {
            midiOut.Send(MidiMessage.ChangeControl(1, modulation, midiChannel).RawData);
        }

        public void SetPitchBend(int pitchBendValue)
        {
            // Set limits
            if (pitchBendValue > 16383)
                pitchBendValue = 16383;
            if (pitchBendValue < 0)
                pitchBendValue = 0;

            int lsb = pitchBendValue & 0b1111111;
            int msb = (pitchBendValue & 0b1111111_0000000) >> 7;
            int status = 0b1110 << 4;

            MidiMessage message = new MidiMessage(status, lsb, msb);
            midiOut.Send(message.RawData);
        }

        public void SetPitchNoBend()
        {
            SetPitchBend(8192);
        }

        /*
         * 0 - 16383
         * 8192
         */ 
    }
}
