namespace HANDMIsTestSuite.Modules.GuideModule
{
    public class GuideString
    {
        private string text;
        private int opaqueTime;
        private int transparency;
        private int r;
        private int g;
        private int b;

        public string Text { get => text; set => text = value; }
        public int OpaqueTime { get => opaqueTime; set => opaqueTime = value; }
        public int Transparency
        {
            get { return transparency; }
            set { if(value < 0) { transparency = 0; } else if(value > 255) { transparency = 255; } else { transparency = value; }  }
        }

        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }

        public GuideString(string text, int opaqueTime, int r, int g, int b)
        {
            this.text = text;
            this.opaqueTime = opaqueTime;
            Transparency = 0;
            R = r;
            G = g;
            B = b;
        }

        public GuideString(string text, int opaqueTime)
        {
            this.text = text;
            this.opaqueTime = opaqueTime;
            Transparency = 0;
            R = 0xFF;
            G = 0xFF;
            B = 0xFF;
        }
    }
}
