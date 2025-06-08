using System.Text.Json;
using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Services
{
    public interface IOpenMeteoService
    {
        Task<OpenMeteoResponse?> GetWeatherForecastAsync(decimal latitude, decimal longitude, int forecastDays = 7);
    }

    public class OpenMeteoService : IOpenMeteoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenMeteoService> _logger;

        public OpenMeteoService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenMeteoService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<OpenMeteoResponse?> GetWeatherForecastAsync(decimal latitude, decimal longitude, int forecastDays = 7)
        {
            try
            {
                var baseUrl = _configuration["WeatherSettings:OpenMeteoBaseUrl"];
                var url = $"{baseUrl}?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min&forecast_days={forecastDays}&timezone=auto";

                _logger.LogInformation("Fazendo requisição para Open-Meteo API: {Url}", url);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenMeteoResponse>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return weatherData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados da Open-Meteo API para latitude {Latitude}, longitude {Longitude}", latitude, longitude);
                return null;
            }
        }
    }
}

