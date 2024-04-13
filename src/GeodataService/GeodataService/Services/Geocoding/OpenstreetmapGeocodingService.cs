using GeodataService.Models;
using System.Text.Json;

namespace GeodataService.Services.Geocoding
{
    public class OpenstreetmapGeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenstreetmapGeocodingService> _logger;

        public OpenstreetmapGeocodingService(HttpClient httpClient, ILogger<OpenstreetmapGeocodingService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "GeodataService");
        }

        public async Task<Geolocation> GetGeolocationAsync(string country, string city, string street)
        {
            try
            {
                string apiUrl = $"https://nominatim.openstreetmap.org/search?country={country}&city=" +
                                $"{city}&street={street}&format=json&limit=1";

                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve geolocation data. HTTP status code: {0}",
                                    response.StatusCode);
                    return null;
                }

                using var responseStream = await response.Content.ReadAsStreamAsync();
                var jsonDocument = await JsonDocument.ParseAsync(responseStream);

                var firstResult = jsonDocument.RootElement.EnumerateArray().FirstOrDefault();

                if (firstResult.ValueKind != JsonValueKind.Null)
                {
                    if (double.TryParse(firstResult.GetProperty("lat").GetString(), out double latitude) &&
                        double.TryParse(firstResult.GetProperty("lon").GetString(), out double longitude))
                    {
                        var geolocation = new Geolocation
                        {
                            Latitude = latitude,
                            Longitude = longitude
                        };

                        _logger.LogInformation("Received geolocation response: {0} {1}",
                                                geolocation.Latitude,
                                                geolocation.Longitude);

                        return geolocation;
                    }
                    else
                    {
                        _logger.LogError("Failed to parse latitude or longitude from JSON response.");
                        return null;
                    }
                }
                else
                {
                    _logger.LogError("Failed to retrieve geolocation data. No results found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving geolocation data.");
                throw;
            }
        }
    }
}
