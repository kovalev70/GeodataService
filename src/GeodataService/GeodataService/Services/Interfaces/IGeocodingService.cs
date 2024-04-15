using GeodataService.Models;

namespace GeodataService.Services.Interfaces
{
    public interface IGeocodingService
    {
        Task<Geolocation> GetGeolocationAsync(string country, string city, string street);
    }
}
