using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IOpenMeteoService _openMeteoService;
        private readonly IHeatRiskService _heatRiskService;
        private readonly ITipsService _tipsService;
        private readonly ICityService _cityService;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(
            IOpenMeteoService openMeteoService,
            IHeatRiskService heatRiskService,
            ITipsService tipsService,
            ICityService cityService,
            ILogger<WeatherService> logger)
        {
            _openMeteoService = openMeteoService;
            _heatRiskService = heatRiskService;
            _tipsService = tipsService;
            _cityService = cityService;
            _logger = logger;
        }

        public async Task<WeatherResponse?> GetWeatherForecastWithTipsAsync(int cityId)
        {
            try
            {
                var city = await _cityService.GetCityByIdAsync(cityId);
                if (city == null)
                {
                    _logger.LogWarning("Cidade não encontrada: {CityId}", cityId);
                    return null;
                }

                var forecast = await _openMeteoService.GetWeatherForecastAsync(city.Latitude ?? 0, city.Longitude ?? 0);
                if (forecast == null)
                {
                    _logger.LogError("Erro ao obter previsão para cidade: {CityName}", city.Nome);
                    return null;
                }

                var processedForecasts = _heatRiskService.ProcessWeatherForecast(forecast);
                var hasExtremeRisk = _heatRiskService.HasExtremeHeatRisk(processedForecasts);

                var tips = new List<Dica>();
                if (hasExtremeRisk)
                {
                    tips = await _tipsService.GetTipsByRiskLevelAsync("risco");
                }
                else
                {
                    var maxTemp = processedForecasts.Max(f => f.TemperatureMax);
                    var riskLevel = maxTemp >= 30 ? "alerta" : "normal";
                    tips = await _tipsService.GetTipsByRiskLevelAsync(riskLevel);
                }

                return new WeatherResponse
                {
                    City = city,
                    Forecasts = processedForecasts,
                    HasExtremeHeatRisk = hasExtremeRisk,
                    Tips = tips,
                    LastUpdated = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter previsão completa para cidade: {CityId}", cityId);
                return null;
            }
        }

        public async Task<List<WeatherAlert>> GetActiveAlertsAsync()
        {
            try
            {
                var cities = await _cityService.GetAllCitiesAsync();
                var alerts = new List<WeatherAlert>();

                foreach (var city in cities)
                {
                    try
                    {
                        var forecast = await _openMeteoService.GetWeatherForecastAsync(city.Latitude ?? 0, city.Longitude ?? 0);
                        if (forecast?.Daily != null)
                        {
                            var processedForecasts = _heatRiskService.ProcessWeatherForecast(forecast);
                            var extremeRiskDays = processedForecasts.Where(f => f.RiskLevel == "risco").ToList();

                            foreach (var riskDay in extremeRiskDays)
                            {
                                alerts.Add(new WeatherAlert
                                {
                                    CityId = city.Id,
                                    CityName = city.Nome,
                                    RiskLevel = riskDay.RiskLevel,
                                    MaxTemperature = riskDay.TemperatureMax,
                                    AlertDate = riskDay.Date,
                                    Message = $"Alerta de calor extremo em {city.Nome}. Temperatura máxima prevista: {riskDay.TemperatureMax}°C"
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar alertas para cidade: {CityName}", city.Nome);
                    }
                }

                return alerts.OrderBy(a => a.AlertDate).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter alertas ativos");
                return new List<WeatherAlert>();
            }
        }
    }
}

