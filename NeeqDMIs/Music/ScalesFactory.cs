using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeeqDMIs.Music
{
    /// <summary>
    /// A static class containing the most typical scales, to avoid re-instantiation and delays.
    /// </summary>
    public static class ScalesFactory
    {
        public static List<Scale> GetList()
        {
            List<Scale> list = new List<Scale>()
            {
            Cmaj, sCmaj, Dmaj, sDmaj, Emaj, Fmaj, sFmaj, Gmaj, sGmaj, Amaj, sAmaj, Bmaj,
            Cmin, sCmin, Dmin, sDmin, Emin, Fmin, sFmin, Gmin, sGmin, Amin, sAmin, Bmin,
            Chrom
            };
            return list;
        }

        public static readonly Scale Cmaj = new Scale(AbsNotes.C, ScaleCodes.maj);
        public static readonly Scale sCmaj = new Scale(AbsNotes.sC, ScaleCodes.maj);
        public static readonly Scale Dmaj = new Scale(AbsNotes.D, ScaleCodes.maj);
        public static readonly Scale sDmaj = new Scale(AbsNotes.sD, ScaleCodes.maj);
        public static readonly Scale Emaj = new Scale(AbsNotes.E, ScaleCodes.maj);
        public static readonly Scale Fmaj = new Scale(AbsNotes.F, ScaleCodes.maj);
        public static readonly Scale sFmaj = new Scale(AbsNotes.sF, ScaleCodes.maj);
        public static readonly Scale Gmaj = new Scale(AbsNotes.G, ScaleCodes.maj);
        public static readonly Scale sGmaj = new Scale(AbsNotes.sG, ScaleCodes.maj);
        public static readonly Scale Amaj = new Scale(AbsNotes.A, ScaleCodes.maj);
        public static readonly Scale sAmaj = new Scale(AbsNotes.sA, ScaleCodes.maj);
        public static readonly Scale Bmaj = new Scale(AbsNotes.B, ScaleCodes.maj);

        public static readonly Scale Cmin = new Scale(AbsNotes.C, ScaleCodes.min);
        public static readonly Scale sCmin = new Scale(AbsNotes.sC, ScaleCodes.min);
        public static readonly Scale Dmin = new Scale(AbsNotes.D, ScaleCodes.min);
        public static readonly Scale sDmin = new Scale(AbsNotes.sD, ScaleCodes.min);
        public static readonly Scale Emin = new Scale(AbsNotes.E, ScaleCodes.min);
        public static readonly Scale Fmin = new Scale(AbsNotes.F, ScaleCodes.min);
        public static readonly Scale sFmin = new Scale(AbsNotes.sF, ScaleCodes.min);
        public static readonly Scale Gmin = new Scale(AbsNotes.G, ScaleCodes.min);
        public static readonly Scale sGmin = new Scale(AbsNotes.sG, ScaleCodes.min);
        public static readonly Scale Amin = new Scale(AbsNotes.A, ScaleCodes.min);
        public static readonly Scale sAmin = new Scale(AbsNotes.sA, ScaleCodes.min);
        public static readonly Scale Bmin = new Scale(AbsNotes.B, ScaleCodes.min);

        public static readonly Scale Chrom = new Scale(AbsNotes.C, ScaleCodes.chrom);

    }
}
