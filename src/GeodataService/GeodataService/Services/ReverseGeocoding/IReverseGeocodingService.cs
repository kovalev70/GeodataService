using GeodataService.Models;

namespace GeodataService.Services.ReverseGeocoding
{
    public interface IReverseGeocodingService
    {
        Task<List<ReverseGeocodingAddress>> ReverseGeocodeAsync(double latitude, double longitude);
    }
}
