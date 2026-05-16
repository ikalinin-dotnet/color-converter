namespace Covide.Web.Domain
{
    public struct RgbColor
    {
        public int R { get; }
        public int G { get; }
        public int B { get; }

        public double RNormalized => (double)R / 255;
        public double GNormalized => (double)G / 255;
        public double BNormalized => (double)B / 255;

        public RgbColor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
