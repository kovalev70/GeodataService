using GeodataService.Models;
using Dadata;
using System.Text;
using GeodataService.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace GeodataService.Services.ReverseGeocoding
{
    public class DadataReverseGeocodingService : IReverseGeocodingService
    {
        private readonly string? _token;
        private readonly ILogger<DadataReverseGeocodingService> _logger;

        public DadataReverseGeocodingService(IConfiguration configuration, ILogger<DadataReverseGeocodingService> logger)
        {
            _token = configuration["DadataSettings:Token"];
            _logger = logger;
        }

        public async Task<List<ReverseGeocodingAddress>> ReverseGeocodeAsync(double latitude, double longitude)
        {
            try
            {
                var addresses = await GetAddressesAsync(latitude, longitude);
                LogAddresses(addresses);
                return addresses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while performing reverse geocoding");
                throw;
            }
        }

        private async Task<List<ReverseGeocodingAddress>> GetAddressesAsync(double latitude, double longitude)
        {
            var addresses = new List<ReverseGeocodingAddress>();
            var api = new SuggestClientAsync(_token);
            var result = await api.Geolocate(lat: latitude, lon: longitude);

            foreach (var address in result.suggestions)
            {
                addresses.Add(new ReverseGeocodingAddress
                {
                    Region = address.data.region,
                    City = address.data.city,
                    Street = address.data.street,
                    House = address.data.house,
                });
            }

            return addresses;
        }

        private void LogAddresses(List<ReverseGeocodingAddress> addresses)
        {
            var logStringBuilder = new StringBuilder();

            foreach (var address in addresses)
            {
                logStringBuilder.AppendLine($"Region: {address.Region}, " +
                                            $"City: {address.City}, " +
                                            $"Street: {address.Street}, " +
                                            $"House: {address.House}");
            }

            _logger.LogInformation("Received reverse geocoding response:\n{0}", logStringBuilder.ToString());
        }
    }
}