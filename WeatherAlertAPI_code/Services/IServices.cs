using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse?> GetWeatherForecastWithTipsAsync(int cityId);
        Task<List<WeatherAlert>> GetActiveAlertsAsync();
    }

    public interface ITipsService
    {
        Task<List<Dica>> GetAllTipsAsync();
        Task<List<Dica>> GetTipsByRiskLevelAsync(string riskLevel);
    }
}

