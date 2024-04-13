using GeodataService.Models;
using GeodataService.Services.Geocoding;
using GeodataService.Services.ReverseGeocoding;
using Microsoft.AspNetCore.Mvc;

namespace GeodataService.Controllers
{
    [Route("api/geodata")]
    [ApiController]
    public class GeodataController : ControllerBase
    {
        private readonly IGeocodingService _geocodingService;
        private readonly IReverseGeocodingService _reverseGeocodingService;

        public GeodataController(IReverseGeocodingService reverseGeocodingService,
            IGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;
            _reverseGeocodingService = reverseGeocodingService;
        }

        [HttpGet("addresses")]
        [ProducesResponseType(typeof(List<ReverseGeocodingAddress>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAddresses([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var addresses = await _reverseGeocodingService.ReverseGeocodeAsync(latitude, longitude);
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("geolocation")]
        [ProducesResponseType(typeof(Geolocation), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetGeolocation([FromQuery] string country, [FromQuery] string city, [FromQuery] string street)
        {
            try
            {
                var geolocation = await _geocodingService.GetGeolocationAsync(country, city, street);
                return Ok(geolocation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
