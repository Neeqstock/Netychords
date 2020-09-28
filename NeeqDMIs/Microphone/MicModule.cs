using NAudio.Wave;
using System;

namespace NeeqDMIs.Microphone
{
    public class MicModule : AggregatorListener
    {
        #region Untouchable fields
        private int sampleRate = 8000; // 8 kHz
        private int channels = 1; // mono

        private short sample;
        private float sample32;

        private int deviceNumber = 0;

        private bool isOverThreshold = false;

        private float maxLevel = 0;
        private float minLevel = 0;

        private float lastSample;
        private float lastMax = 0;
        private float lastMin = 0;
        #endregion

        #region Tweakable settings
        private int playThresholdLow = 50;    // Default value
        private int playThresholdHigh = 200;  // Default value
        private int aggregatorSize = 15;      // Default value

        public int PlayThresholdLow
        {
            get
            {
                return playThresholdLow;
            }

            set
            {
                playThresholdLow = value;
            }
        }
        public int PlayThresholdHigh
        {
            get
            {
                return playThresholdHigh;
            }

            set
            {
                playThresholdHigh = value;
            }
        }
        public int AggregatorSize
        {
            get
            {
                return aggregatorSize;
            }

            set
            {
                aggregatorSize = value;
            }
        }
        #endregion

        #region Auto-generated tools
        private SampleAggregator sampleAggregator;
        private WaveIn waveIn = null;
        private WaveOut waveOut = null;
        #endregion

        #region Interface
        private MicControllerListener listener;
        public MicControllerListener Listener { get { return listener; } set { listener = value; } }
        #endregion

        private const bool enabled = false;

        public MicModule()
        {
            if (enabled)
            {
                sampleAggregator = new SampleAggregator(this, aggregatorSize);

                waveIn = new WaveIn();
                waveIn.DeviceNumber = deviceNumber;
                waveIn.DataAvailable += waveIn_DataAvailable;

                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                waveIn.StartRecording();
            }
        }

        public MicModule(int playThresholdLow, int playThresholdHigh, int aggregatorSize)
        {

            PlayThresholdLow = playThresholdLow;
            PlayThresholdHigh = playThresholdHigh;

            sampleAggregator = new SampleAggregator(this, aggregatorSize);

            waveIn = new WaveIn();
            waveIn.DeviceNumber = deviceNumber;
            waveIn.DataAvailable += waveIn_DataAvailable;
            
            waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
            waveIn.StartRecording();
        }

        #region Internal functions
        private void stopRec()
        {
            waveIn.StopRecording();
        }

        private void playWaveOut()
        {
            waveOut.Play();
        }

        private void stopWaveOut()
        {
            waveOut.Stop();
        }

        private void waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            // stop playback
            if (waveOut != null)
                waveOut.Stop();

            // dispose of wave input
            if (waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }
        }

        private void wavePlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            // stop recording
            if (waveIn != null)
                waveIn.StopRecording();

            // dispose of wave output
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }

        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                sample = (short)((e.Buffer[index + 1] << 8) |
                                        e.Buffer[index + 0]);
                sample32 = sample; // 32768f;
                lastSample = Math.Abs(sample32);

                sampleAggregator.push(lastSample);

            }
        }
        #endregion

        public float getMicLevel()
        {
            return lastMax;
        }

        public float getMicMax()
        {
            return maxLevel;
        }

        public float getMicMin()
        {
            return minLevel;
        }

        public void aggregatorReached(float min, float max, float avg)
        {
            bool overThresh;

            if (isOverThreshold)
            {
                overThresh = true;
                if (max < PlayThresholdLow)
                {
                    overThresh = false;
                    if (listener != null)
                        listener.processStopBlowing();
                }
                    
            }
            else
            {
                overThresh = false;
                if (max > PlayThresholdHigh)
                {
                    overThresh = true;
                    if (listener!= null)
                        listener.processStartBlowing();
                }
                    
            }
            
            isOverThreshold = overThresh;
           
            if (maxLevel < max)
                maxLevel = max;
            if (minLevel > min)
                minLevel = min;

            lastMax = max;
            lastMin = min;

        }
    }
}
