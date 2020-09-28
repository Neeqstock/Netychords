namespace NeeqDMIs.Utils
{
    public class ValueNormalizerLong
    {
        public long ParamMax { get; set; } = 0;
        public long ChannelMax { get; set; } = 0;

        public long Normalize (long value, long channelMax, long paramMax)
        {
            return (long)((value / (double)channelMax) * paramMax);
        }

        public long Normalize (long value, long channelMax)
        {
            return (long)((value / (double)channelMax) * ParamMax);
        }

        public long Normalize (long value)
        {
            return (long)((value / (double)ChannelMax) * ParamMax);
        }

        public ValueNormalizerLong()
        {
        }

        public ValueNormalizerLong(long paramMax)
        {
            ParamMax = paramMax;
        }

        public ValueNormalizerLong(long paramMax, long channelMax) : this(paramMax)
        {
            ChannelMax = channelMax;
        }
    }

    public class ValueNormalizerDouble
    {
        public double ParamMax { get; set; } = 0;
        public double ChannelMax { get; set; } = 0;

        public double Normalize(double value, double channelMax, double paramMax)
        {
            return ((value / channelMax) * paramMax);
        }

        public double Normalize(double value, double channelMax)
        {
            return ((value / channelMax) * ParamMax);
        }

        public double Normalize(double value)
        {
            return ((value / ChannelMax) * ParamMax);
        }

        public ValueNormalizerDouble()
        {
        }

        public ValueNormalizerDouble(double paramMax)
        {
            ParamMax = paramMax;
        }

        public ValueNormalizerDouble(double paramMax, double channelMax) : this(paramMax)
        {
            ChannelMax = channelMax;
        }
    }
}
