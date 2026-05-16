using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Covide.Web.Domain;

namespace Covide.Web.ColorSpaces
{
    public class HexColorSpace : IColorSpace<string>
    {
        private static readonly Regex HexPattern = new Regex("^[0-9a-fA-F]{6}$", RegexOptions.Compiled);

        public bool IsValid(string hex) => HexPattern.IsMatch(hex ?? string.Empty);

        public string FromRgb(RgbColor rgb)
        {
            return $"{rgb.R:x2}{rgb.G:x2}{rgb.B:x2}";
        }

        public RgbColor ToRgb(string hex)
        {
            int r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            return new RgbColor(r, g, b);
        }
    }
}
