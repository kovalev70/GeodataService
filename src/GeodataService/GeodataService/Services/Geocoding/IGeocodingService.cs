using GeodataService.Models;

namespace GeodataService.Services.Geocoding
{
    public interface IGeocodingService
    {
        Task<Geolocation> GetGeolocationAsync(string country, string city, string street);
    }
}
