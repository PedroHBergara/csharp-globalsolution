namespace WeatherAlertAPI.Models
{
    public class OpenMeteoResponse
    {
        public Daily? Daily { get; set; }
    }

    public class Daily
    {
        public List<string>? Time { get; set; }
        public List<double>? Temperature_2m_max { get; set; }
        public List<double>? Temperature_2m_min { get; set; }
    }
}

