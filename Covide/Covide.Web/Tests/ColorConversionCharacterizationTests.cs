using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Covide.Web.Tests
{
    public class ColorConversionCharacterizationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ColorConversionCharacterizationTests(WebApplicationFactory<Startup> factory)
        {
            using (var scope = factory.Services.CreateScope())
            {
                CovideDataSeeder.Seed(scope.ServiceProvider);
            }
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("8e35ef",
            "{\"hexTriplet\":\"8e35ef\",\"name\":\"Purple\",\"rgbDecimal\":[142,53,239],\"rgbPercentage\":[55.7,20.8,93.7],\"cmyk\":[41,78,0,6],\"hsl\":[269,85.3,57.3],\"hsv\":[269,85.3,93.7],\"xyz\":[28.008,14.529,82.99]}")]
        [InlineData("ff0000",
            "{\"hexTriplet\":\"ff0000\",\"name\":\"Red\",\"rgbDecimal\":[255,0,0],\"rgbPercentage\":[100,0,0],\"cmyk\":[0,100,100,0],\"hsl\":[0,100,50],\"hsv\":[0,100,100],\"xyz\":[41.24,21.26,1.93]}")]
        [InlineData("123456",
            "{\"hexTriplet\":\"123456\",\"name\":null,\"rgbDecimal\":[18,52,86],\"rgbPercentage\":[7.1,20.4,33.7],\"cmyk\":[79,40,0,66],\"hsl\":[210,65.4,20.4],\"hsv\":[210,65.4,33.7],\"xyz\":[3.157,3.256,9.266]}")]
        public async Task Get_ValidHex_ReturnsExpectedJson(string hex, string expectedJson)
        {
            var response = await _client.GetAsync($"/{hex}");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(NormalizeJson(expectedJson), NormalizeJson(body));
        }

        [Fact]
        public async Task Get_InvalidHex_Returns400()
        {
            var response = await _client.GetAsync("/zzzzzz");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static string NormalizeJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(doc.RootElement);
        }
    }
}
