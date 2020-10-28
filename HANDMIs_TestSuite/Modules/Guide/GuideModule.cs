using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace HANDMIsTestSuite.Modules.GuideModule
{
    public delegate void OnFinish();

    public class GuideModule
    {
        private const int ratio = 3;
        public event OnFinish onFinish;
        
        private List<TextBlock> textBlocks;
        private Queue<GuideString> strings = new Queue<GuideString>();
        public List<TextBlock> TextBlocks { get => textBlocks; set => textBlocks = value; }
        public bool IsLocked { get => isLocked; }

        private DispatcherTimer RiseTimer;
        private DispatcherTimer OpaqueTimer;
        private DispatcherTimer FallTimer;

        private int riseInterval;
        private int fallInterval;

        private bool isLocked = false;

        public GuideModule(List<TextBlock> textBlocks, int riseInterval, int fallInterval)
        {
            this.textBlocks = textBlocks;
            this.riseInterval = riseInterval;
            this.fallInterval = fallInterval;
        }

        public void Reset()
        {
            if (RiseTimer != null)
                RiseTimer.Stop();
            if (OpaqueTimer != null)
                OpaqueTimer.Stop();
            if (FallTimer != null)
                FallTimer.Stop();
            strings.Clear();

            foreach (TextBlock textBlock in TextBlocks)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Transparent);
            }
        }

        public void AddString(string text, int opaqueTime, int R, int G, int B)
        {
            strings.Enqueue(new GuideString(text, opaqueTime, R, G, B));
        }

        public void AddString(GuideString guideString)
        {
            strings.Enqueue(guideString);
        }

        public void AddStrings(List<GuideString> stringsList)
        {
            if(stringsList != null)
            {
                foreach (GuideString gs in stringsList)
                {
                    strings.Enqueue(gs);
                }
            }
        }

        public void StartStrings()
        {
            if(strings.Count > 0)
            {
                isLocked = true;
                RiseTimer = new DispatcherTimer(DispatcherPriority.Render);
                OpaqueTimer = new DispatcherTimer(DispatcherPriority.Render);
                FallTimer = new DispatcherTimer(DispatcherPriority.Render);
                OpaqueTimer.Interval = new TimeSpan(strings.Peek().OpaqueTime * 1000000);
                RiseTimer.Interval = new TimeSpan(riseInterval);
                FallTimer.Interval = new TimeSpan(fallInterval);

                RiseTimer.Tick += new EventHandler(RiseTimer_Tick);
                OpaqueTimer.Tick += new EventHandler(OpaqueTimer_Tick);
                FallTimer.Tick += new EventHandler(FallTimer_Tick);

                foreach (TextBlock textBlock in TextBlocks)
                {
                    textBlock.Foreground = new SolidColorBrush(Colors.Transparent);
                    textBlock.Text = strings.Peek().Text;
                }                    

                RiseTimer.Start();
            }
            else
            {
                isLocked = false;
                onFinish?.Invoke();
                onFinish = null;
            }
        }

        private void RiseTimer_Tick(object sender, EventArgs e)
        {
            strings.Peek().Transparency += ratio;

            foreach (TextBlock textBlock in TextBlocks)
            {
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb((byte)strings.Peek().Transparency, (byte)strings.Peek().R, (byte)strings.Peek().G, (byte)strings.Peek().B));

            }

            if (strings.Peek().Transparency >= 255)
            {
                RiseTimer.Stop();
                OpaqueTimer.Start();
            }
        }
        private void FallTimer_Tick(object sender, EventArgs e)
        {
            if(strings.Count > 0)
            {
                strings.Peek().Transparency -= ratio;

                foreach (TextBlock textBlock in TextBlocks)
                {
                    textBlock.Foreground = new SolidColorBrush(Color.FromArgb((byte)strings.Peek().Transparency, (byte)strings.Peek().R, (byte)strings.Peek().G, (byte)strings.Peek().B));

                }
                if (strings.Peek().Transparency <= 0)
                {
                    FallTimer.Stop();
                    strings.Dequeue();
                    StartStrings();
                }
            }
            else
            {
                isLocked = false;
                onFinish();
                onFinish = null;
            }
        }

        private void OpaqueTimer_Tick(object sender, EventArgs e)
        {
            OpaqueTimer.Stop();
            FallTimer.Start();
        }


    }

    public enum OpacityStates
    {
        Transparent,
        Rising,
        Opaque,
        Falling
    }
}
