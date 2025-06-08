using WeatherAlertAPI.Services;

namespace WeatherAlertAPI.Tests
{
    public class OpenMeteoServiceTest
    {
        public static async Task TestOpenMeteoIntegration(IOpenMeteoService openMeteoService)
        {
            // Teste com coordenadas de São Paulo
            var latitude = -23.5505m;
            var longitude = -46.6333m;

            Console.WriteLine($"Testando integração com Open-Meteo API para São Paulo (lat: {latitude}, lon: {longitude})");

            var result = await openMeteoService.GetWeatherForecastAsync(latitude, longitude);

            if (result != null && result.Daily != null)
            {
                Console.WriteLine("✅ Integração com Open-Meteo API funcionando!");
                Console.WriteLine($"Dados recebidos para {result.Daily.Time?.Count ?? 0} dias");
                
                if (result.Daily.Time != null && result.Daily.Temperature_2m_max != null && result.Daily.Temperature_2m_min != null)
                {
                    for (int i = 0; i < result.Daily.Time.Count && i < result.Daily.Temperature_2m_max.Count && i < result.Daily.Temperature_2m_min.Count; i++)
                    {
                        Console.WriteLine($"Data: {result.Daily.Time[i]}, Temp Max: {result.Daily.Temperature_2m_max[i]}°C, Temp Min: {result.Daily.Temperature_2m_min[i]}°C");
                    }
                }
            }
            else
            {
                Console.WriteLine(" Erro na integração com Open-Meteo API");
            }
        }
    }
}

