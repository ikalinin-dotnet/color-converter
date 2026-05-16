using Covide.Web.ColorSpaces;
using Covide.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Covide.Web.Controllers
{
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private static readonly HexColorSpace _hexColorSpace = new HexColorSpace();
        private readonly ColorConversionService _conversionService;

        public ConversionController(ColorConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpGet("{hex}")]
        public ActionResult Get([FromRoute] string hex)
        {
            if (!_hexColorSpace.IsValid(hex))
            {
                return BadRequest();
            }

            return Ok(_conversionService.Convert(hex));
        }
    }
}
