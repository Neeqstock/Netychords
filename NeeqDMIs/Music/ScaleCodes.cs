using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.Music
{
    /// <summary>
    /// Enumeration of scale codes (minor, major...). Extension methods provide the "rules" for the scale.
    /// </summary>
    public enum ScaleCodes
    {
        min,
        maj,
        chrom
    }

    static class ScaleCodesMethods
    {
        /// <summary>
        /// Returns the scale rule, in relative pitch positions.
        /// </summary>
        /// <param name="scaleCode"></param>
        /// <returns></returns>
        public static List<int> GetRule(this ScaleCodes scaleCode)
        {
            switch (scaleCode)
            {
                case ScaleCodes.maj:
                    return new List<int>() { 0, 2, 4, 5, 7, 9, 11 };
                case ScaleCodes.min:
                    return new List<int>() { 0, 2, 3, 5, 7, 8, 10 };
                case ScaleCodes.chrom:
                    return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                default:
                    return new List<int>() { };
            }
        }
    }
}
