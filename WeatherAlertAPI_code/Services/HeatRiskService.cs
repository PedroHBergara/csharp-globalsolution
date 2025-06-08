using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Services
{
    public interface IHeatRiskService
    {
        string DetermineRiskLevel(double maxTemperature);
        bool HasExtremeHeatRisk(List<WeatherForecast> forecast);
        List<WeatherForecast> ProcessWeatherForecast(OpenMeteoResponse openMeteoData);
    }

    public class HeatRiskService : IHeatRiskService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HeatRiskService> _logger;

        public HeatRiskService(IConfiguration configuration, ILogger<HeatRiskService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string DetermineRiskLevel(double maxTemperature)
        {
            var extremeHeatThreshold = _configuration.GetValue<double>("WeatherSettings:ExtremeHeatThreshold", 35.0);

            if (maxTemperature >= extremeHeatThreshold)
            {
                return "risco";
            }
            else if (maxTemperature >= extremeHeatThreshold - 5) // 30°C ou mais
            {
                return "alerta";
            }
            else
            {
                return "normal";
            }
        }

        public bool HasExtremeHeatRisk(List<WeatherForecast> forecast)
        {
            return forecast.Any(f => f.RiskLevel == "risco");
        }

        public List<WeatherForecast> ProcessWeatherForecast(OpenMeteoResponse openMeteoData)
        {
            var forecasts = new List<WeatherForecast>();

            if (openMeteoData?.Daily?.Time != null && 
                openMeteoData.Daily.Temperature_2m_max != null && 
                openMeteoData.Daily.Temperature_2m_min != null)
            {
                for (int i = 0; i < openMeteoData.Daily.Time.Count && 
                             i < openMeteoData.Daily.Temperature_2m_max.Count && 
                             i < openMeteoData.Daily.Temperature_2m_min.Count; i++)
                {
                    var maxTemp = openMeteoData.Daily.Temperature_2m_max[i];
                    var minTemp = openMeteoData.Daily.Temperature_2m_min[i];
                    var riskLevel = DetermineRiskLevel(maxTemp);

                    if (DateTime.TryParse(openMeteoData.Daily.Time[i], out var date))
                    {
                        forecasts.Add(new WeatherForecast
                        {
                            Date = date,
                            TemperatureMax = (decimal)maxTemp,
                            TemperatureMin = (decimal)minTemp,
                            RiskLevel = riskLevel
                        });
                    }
                }
            }

            _logger.LogInformation("Processados {Count} dias de previsão. Dias com risco extremo: {RiskDays}", 
                forecasts.Count, forecasts.Count(f => f.RiskLevel == "risco"));

            return forecasts;
        }
    }
}

