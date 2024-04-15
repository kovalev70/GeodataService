using GeodataService.Models;

namespace GeodataService.Services.Interfaces
{
    public interface IReverseGeocodingService
    {
        Task<List<ReverseGeocodingAddress>> ReverseGeocodeAsync(double latitude, double longitude);
    }
}
