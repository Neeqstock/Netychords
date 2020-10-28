using HANDMIsTestSuite.Modules.GuideModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows.Media;

namespace HANDMIsTestSuite
{
    public static class GlobalValues
    {
        static GlobalValues()
        {
            SoundRestInput = new SoundPlayer(HANDMIs_TestSuite.Properties.Resources.Risset);
            SoundTrialStart = new SoundPlayer(HANDMIs_TestSuite.Properties.Resources.Risset600);
            SoundTrialEnd = new SoundPlayer(HANDMIs_TestSuite.Properties.Resources.Risset350);
            SoundTrialFailure = new SoundPlayer(HANDMIs_TestSuite.Properties.Resources._800);

            SoundRestInput.Load();
            SoundTrialStart.Load();
            SoundTrialEnd.Load();
            SoundTrialFailure.Load();

            HalfCursorWidth = CursorWidth / 2;
        }

        public static SoundPlayer SoundRestInput;
        public static SoundPlayer SoundTrialStart;
        public static SoundPlayer SoundTrialEnd;
        public static SoundPlayer SoundTrialFailure;

        public const int SamplingTimerDuration = 1;
        public const int TargetWidth = 200;
        public const int CursorOffset = 15;
        public const int CursorWidth = 4;
        public const int TargetCenterOffset = 50;
        public const int TargetCenterWidth = 4;
        public const double BarTextSeparator = 25;
        /// <summary>
        /// In seconds: how much time is required for a selection to be considered stable.
        /// </summary>
        public const int SelectionTimeRequired = 1;
        /// <summary>
        /// In seconds: for how much time the stability is analysed
        /// </summary>
        public const int StabilityTimeTotal = 4;
        /// <summary>
        /// In seconds: how much time has the tester to make a selection
        /// </summary>
        public const int SelectionTimeMax = 5;
        /// <summary>
        /// In seconds
        /// </summary>
        public const int RestTime = 2;

        public const int TrainingTrialsNumber = 5;
        public const int TotalTrialsNumber = 20;
        public const long BarWidth = 1000;

        /// <summary>
        /// Indicates when the selection timer will declare a fail
        /// </summary>
        public static int SelectionTimerFail { get { return SelectionTimeMax + SelectionTimeRequired; } }
        public static int HeadYawRange { get; set; } = 20;
        public static int HeadPitchRange { get; set; } = 20;
        public static int HeadRollRange { get; set; } = 20;
        public static long VelocityRange { get; set; } = 6000;

        public static readonly Brush ColorTargetLocked = new SolidColorBrush(Color.FromRgb(0x50, 0x50, 0x50));
        public static readonly Brush ColorTargetNotLocked = new SolidColorBrush(Color.FromRgb(0x50, 0x50, 0x50));
        public static readonly Brush ColorTargetSelected = new SolidColorBrush(Color.FromRgb(0x50, 0x50, 0x50));

        public static readonly Brush ColorTargetCenterLocked = new SolidColorBrush(Colors.LightGreen);
        public static readonly Brush ColorTargetCenterNotLocked = new SolidColorBrush(Colors.Coral);
        public static readonly Brush ColorTargetCenterSelected = new SolidColorBrush(Colors.Violet);

        public static readonly Brush ColorTrialsTraining = new SolidColorBrush(Colors.LightCoral);
        public static readonly Brush ColorTrialsMeasured = new SolidColorBrush(Colors.LightGreen);
        public static readonly Brush ColorTrialsRest = new SolidColorBrush(Colors.Yellow);

        public static readonly Brush ColorCursor = new SolidColorBrush(Colors.White);

        public static readonly Brush ColorActive = new SolidColorBrush(Colors.LightGreen);
        public static readonly Brush ColorError = new SolidColorBrush(Colors.LightCoral);
        public static readonly Brush ColorTransparent = new SolidColorBrush(Colors.Transparent);

        public static int HalfCursorWidth { get; private set; }

        public static readonly List<GuideString> GuideExperimentStart_real = new List<GuideString>()
        {
            new GuideString("Starting experiment!", 5),
            new GuideString("Place the white cursor on the red target,", 5),
            new GuideString("and hold it on place as stable as possible", 5),
            new GuideString("for " + SelectionTimeRequired / 1000 + " seconds!", 5)
        };

        public static readonly List<GuideString> GuideExperimentStart = new List<GuideString>()
        {
            new GuideString("Starting experiment!", 5)
        };

        public static readonly List<GuideString> GuideExperimentFinish = new List<GuideString>()
        {
            new GuideString("Experiment finished!", 5)
        };

        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
