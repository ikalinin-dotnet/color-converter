using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Covide.Web.Controllers
{    
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly CovideDataContext _db;

        public ConversionController(CovideDataContext db)
        {
            _db = db;
        }

        [HttpGet("{hex}")]
        public ActionResult Get([FromRoute] string hex)
        {
            string pattern = "^[0-9a-fA-F]{6}$";

            if (!Regex.Match(hex, pattern).Success)
            {
                return BadRequest();
            }

            int red, green, blue;

            red = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            green = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            blue = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            string hexTriplet = hex.ToLower();

            double redPercentage, greenPercentage, bluePercentage;

            redPercentage = Math.Round(red / 2.55, 1);
            greenPercentage = Math.Round(green / 2.55, 1);
            bluePercentage = Math.Round(blue / 2.55, 1);

            double redNormalized, greenNormalized, blueNormalized;

            redNormalized = (double)red / 255;
            greenNormalized = (double)green / 255;
            blueNormalized = (double)blue / 255;

            double cyan, magenta, yellow, key;

            key = 1 - Math.Max(redNormalized, Math.Max(greenNormalized, blueNormalized));
            cyan = (1 - redNormalized - key) / (1 - key);
            magenta = (1 - greenNormalized - key) / (1 - key);
            yellow = (1 - blueNormalized - key) / (1 - key);

            int cyanPercentage, magentaPercentage, yellowPercentage, keyPercentage;

            keyPercentage = (int)Math.Round(key * 100);
            cyanPercentage = (int)Math.Round(cyan * 100);
            magentaPercentage = (int)Math.Round(magenta * 100);
            yellowPercentage = (int)Math.Round(yellow * 100);

            double cmax, cmin, delta;

            cmax = Math.Max(redNormalized, Math.Max(greenNormalized, blueNormalized));
            cmin = Math.Min(redNormalized, Math.Min(greenNormalized, blueNormalized));
            delta = cmax - cmin;

            double hue, saturation, lightness, value;            

            lightness = (cmax + cmin) / 2;

            hue = delta == 0 ? 0
                : cmax == redNormalized ? (greenNormalized - blueNormalized) / delta % 6
                : cmax == greenNormalized ? (blueNormalized - redNormalized) / delta + 2
                : (redNormalized - greenNormalized) / delta + 4;
            hue *= 60;
            hue = hue < 0 ? hue += 360 : hue;
            hue = Math.Round(hue);

            saturation = delta == 0 ? 0
                : delta / (1 - Math.Abs(2 * lightness - 1));
            saturation *= 100;
            saturation = Math.Round(saturation, 1);

            lightness *= 100;
            lightness = Math.Round(lightness, 1);

            value = cmax * 100;
            value = Math.Round(value, 1);

            double x, y, z;

            double[] xyz = ComputeXYZ(redNormalized, greenNormalized, blueNormalized);

            x = Math.Round(xyz[0], 3);
            y = Math.Round(xyz[1], 3);
            z = Math.Round(xyz[2], 3);

            string name = _db.ColorCodes.FirstOrDefault(cc => cc.HexTriplet == hex.ToUpper())?.Name;

            var response = new
            {
                hexTriplet,
                name,
                rgbDecimal = new[] { red, green, blue },
                rgbPercentage = new[] { redPercentage, greenPercentage, bluePercentage },
                cmyk = new[] { cyanPercentage, magentaPercentage, yellowPercentage, keyPercentage },
                hsl = new[] { hue, saturation, lightness },
                hsv = new[] { hue, saturation, value },
                xyz = new[] { x, y, z}
            };

            return Ok(response);
        }

        private double[] ComputeXYZ(double r , double g, double b)
        {
            if(r > 0.04045)
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
                b= b / 12.92;
            }

            r *= 100;
            g *= 100;
            b *= 100;

            double x, y, z;

            x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            return new[] { x, y, z };
        }
    }
}
