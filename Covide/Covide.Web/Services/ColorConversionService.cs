using Covide.Web.ColorSpaces;
using Covide.Web.Domain;
using Covide.Web.Dtos;

namespace Covide.Web.Services
{
    public class ColorConversionService
    {
        private readonly IColorNameService _colorNameService;

        private static readonly HexColorSpace _hex = new HexColorSpace();
        private static readonly RgbDecimalColorSpace _rgbDecimal = new RgbDecimalColorSpace();
        private static readonly RgbPercentageColorSpace _rgbPercentage = new RgbPercentageColorSpace();
        private static readonly CmykColorSpace _cmyk = new CmykColorSpace();
        private static readonly HslColorSpace _hsl = new HslColorSpace();
        private static readonly HsvColorSpace _hsv = new HsvColorSpace();
        private static readonly XyzColorSpace _xyz = new XyzColorSpace();

        public ColorConversionService(IColorNameService colorNameService)
        {
            _colorNameService = colorNameService;
        }

        public ColorConversionResponse Convert(string hexInput)
        {
            string hexTriplet = hexInput.ToLower();
            RgbColor rgb = _hex.ToRgb(hexInput);

            return new ColorConversionResponse
            {
                HexTriplet = hexTriplet,
                Name = _colorNameService.GetName(hexInput),
                RgbDecimal = _rgbDecimal.FromRgb(rgb),
                RgbPercentage = _rgbPercentage.FromRgb(rgb),
                Cmyk = _cmyk.FromRgb(rgb),
                Hsl = _hsl.FromRgb(rgb),
                Hsv = _hsv.FromRgb(rgb),
                Xyz = _xyz.FromRgb(rgb)
            };
        }
    }
}
