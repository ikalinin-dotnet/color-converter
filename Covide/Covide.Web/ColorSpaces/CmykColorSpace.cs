using System;
using Covide.Web.Domain;

namespace Covide.Web.ColorSpaces
{
    public class CmykColorSpace : IColorSpace<int[]>
    {
        public int[] FromRgb(RgbColor rgb)
        {
            double r = rgb.RNormalized;
            double g = rgb.GNormalized;
            double b = rgb.BNormalized;

            double key = 1 - Math.Max(r, Math.Max(g, b));
            double cyan = (1 - r - key) / (1 - key);
            double magenta = (1 - g - key) / (1 - key);
            double yellow = (1 - b - key) / (1 - key);

            int c = (int)Math.Round(cyan * 100);
            int m = (int)Math.Round(magenta * 100);
            int y = (int)Math.Round(yellow * 100);
            int k = (int)Math.Round(key * 100);

            return new[] { c, m, y, k };
        }

        public RgbColor ToRgb(int[] value)
        {
            double c = value[0] / 100.0;
            double m = value[1] / 100.0;
            double y = value[2] / 100.0;
            double k = value[3] / 100.0;

            int r = (int)Math.Round(255 * (1 - c) * (1 - k));
            int g = (int)Math.Round(255 * (1 - m) * (1 - k));
            int b = (int)Math.Round(255 * (1 - y) * (1 - k));
            return new RgbColor(r, g, b);
        }
    }
}
