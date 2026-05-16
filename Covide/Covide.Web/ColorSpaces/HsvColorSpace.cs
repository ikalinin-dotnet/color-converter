using System;
using Covide.Web.Domain;

namespace Covide.Web.ColorSpaces
{
    public class HsvColorSpace : IColorSpace<double[]>
    {
        public double[] FromRgb(RgbColor rgb)
        {
            double r = rgb.RNormalized;
            double g = rgb.GNormalized;
            double b = rgb.BNormalized;

            double cmax = Math.Max(r, Math.Max(g, b));
            double cmin = Math.Min(r, Math.Min(g, b));
            double delta = cmax - cmin;

            double lightness = (cmax + cmin) / 2;

            double hue = delta == 0 ? 0
                : cmax == r ? (g - b) / delta % 6
                : cmax == g ? (b - r) / delta + 2
                : (r - g) / delta + 4;
            hue *= 60;
            hue = hue < 0 ? hue += 360 : hue;
            hue = Math.Round(hue);

            double saturation = delta == 0 ? 0
                : delta / (1 - Math.Abs(2 * lightness - 1));
            saturation *= 100;
            saturation = Math.Round(saturation, 1);

            double value = cmax * 100;
            value = Math.Round(value, 1);

            return new[] { hue, saturation, value };
        }

        public RgbColor ToRgb(double[] value)
        {
            throw new NotImplementedException();
        }
    }
}
