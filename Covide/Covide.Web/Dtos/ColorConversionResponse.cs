namespace Covide.Web.Dtos
{
    public class ColorConversionResponse
    {
        public string HexTriplet { get; set; }
        public string Name { get; set; }
        public int[] RgbDecimal { get; set; }
        public double[] RgbPercentage { get; set; }
        public int[] Cmyk { get; set; }
        public double[] Hsl { get; set; }
        public double[] Hsv { get; set; }
        public double[] Xyz { get; set; }
    }
}
