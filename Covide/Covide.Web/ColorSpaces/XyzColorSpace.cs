using System;
using Covide.Web.Domain;

namespace Covide.Web.ColorSpaces
{
    public class XyzColorSpace : IColorSpace<double[]>
    {
        public double[] FromRgb(RgbColor rgb)
        {
            double[] xyz = ComputeXYZ(rgb.RNormalized, rgb.GNormalized, rgb.BNormalized);
            return new[]
            {
                Math.Round(xyz[0], 3),
                Math.Round(xyz[1], 3),
                Math.Round(xyz[2], 3)
            };
        }

        public RgbColor ToRgb(double[] value)
        {
            throw new NotImplementedException();
        }

        private static double[] ComputeXYZ(double r, double g, double b)
        {
            if (r > 0.04045)
            {
                r = Math.Pow((r + 0.055) / 1.055, 2.4);
            }
            else
            {
                r = r / 12.92;
            }

            if (g > 0.04045)
            {
                g = Math.Pow((g + 0.055) / 1.055, 2.4);
            }
            else
            {
                g = g / 12.92;
            }

            if (b > 0.04045)
            {
                b = Math.Pow((b + 0.055) / 1.055, 2.4);
            }
            else
            {
                b = b / 12.92;
            }

            r *= 100;
            g *= 100;
            b *= 100;

            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            return new[] { x, y, z };
        }
    }
}
