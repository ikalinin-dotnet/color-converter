using Covide.Web.Domain;

namespace Covide.Web.ColorSpaces
{
    public class RgbDecimalColorSpace : IColorSpace<int[]>
    {
        public int[] FromRgb(RgbColor rgb)
        {
            return new[] { rgb.R, rgb.G, rgb.B };
        }

        public RgbColor ToRgb(int[] value)
        {
            return new RgbColor(value[0], value[1], value[2]);
        }
    }
}
