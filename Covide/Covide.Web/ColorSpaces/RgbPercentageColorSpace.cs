using System;
using Covide.Web.Domain;

namespace Covide.Web.ColorSpaces
{
    public class RgbPercentageColorSpace : IColorSpace<double[]>
    {
        public double[] FromRgb(RgbColor rgb)
        {
            double r = Math.Round(rgb.R / 2.55, 1);
            double g = Math.Round(rgb.G / 2.55, 1);
            double b = Math.Round(rgb.B / 2.55, 1);
            return new[] { r, g, b };
        }

        public RgbColor ToRgb(double[] value)
        {
            return new RgbColor(
                (int)Math.Round(value[0] * 2.55),
                (int)Math.Round(value[1] * 2.55),
                (int)Math.Round(value[2] * 2.55));
        }
    }
}
