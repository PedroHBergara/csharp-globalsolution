namespace WeatherAlertAPI.Models
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public decimal TemperatureMax { get; set; }
        public decimal TemperatureMin { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public List<string> Tips { get; set; } = new List<string>();
    }

    public class WeatherResponse
    {
        public Cidade City { get; set; } = new Cidade();
        public List<WeatherForecast> Forecasts { get; set; } = new List<WeatherForecast>();
        public bool HasExtremeHeatRisk { get; set; }
        public List<Dica> Tips { get; set; } = new List<Dica>();
        public DateTime LastUpdated { get; set; }
    }
}

