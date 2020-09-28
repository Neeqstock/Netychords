using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerpheus.Controllers.Music
{
    /// <summary>
    /// Conversion utilities for tempo.
    /// </summary>
    public static class TempoUtils
    {
        /// <summary>
        /// Converts from BPM to Milliseconds
        /// </summary>
        /// <param name="bpm"></param>
        /// <returns></returns>
        public static int BpmToMilliseconds(int bpm)
        {
            return 1000 * 60 / bpm; // Basic formula found on StackOverflow -> https://stackoverflow.com/questions/9675031/time-interval-in-ms-from-bpm-midi-tempo
        }

        /// <summary>
        /// Converts from milliseconds to BPM.
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static int MillisecondsToBpm(int milliseconds)
        {
            return 1000 * 60 / milliseconds; // Reverse of the previous formula
        }
    }
}
