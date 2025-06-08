using Microsoft.AspNetCore.Mvc;
using WeatherAlertAPI.Models;
using WeatherAlertAPI.Services;

namespace WeatherAlertAPI.Controllers
{
    /// <summary>
    /// Controlador para previsão do tempo e alertas de calor extremo
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IOpenMeteoService _openMeteoService;
        private readonly IHeatRiskService _heatRiskService;

        public WeatherController(
            IWeatherService weatherService,
            IOpenMeteoService openMeteoService,
            IHeatRiskService heatRiskService)
        {
            _weatherService = weatherService;
            _openMeteoService = openMeteoService;
            _heatRiskService = heatRiskService;
        }

        /// <summary>
        /// Obtém previsão do tempo completa com dicas preventivas para uma cidade
        /// </summary>
        /// <param name="cityId">ID da cidade</param>
        /// <returns>Previsão do tempo com alertas e dicas preventivas</returns>
        /// <response code="200">Previsão obtida com sucesso</response>
        /// <response code="404">Cidade não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("forecast/{cityId}")]
        [ProducesResponseType(typeof(WeatherResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WeatherResponse>> GetWeatherForecast(int cityId)
        {
            try
            {
                var weatherResponse = await _weatherService.GetWeatherForecastWithTipsAsync(cityId);
                if (weatherResponse == null)
                {
                    return NotFound(new { message = "Cidade não encontrada ou erro ao obter previsão" });
                }
                return Ok(weatherResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém previsão do tempo por coordenadas geográficas
        /// </summary>
        /// <param name="latitude">Latitude da localização</param>
        /// <param name="longitude">Longitude da localização</param>
        /// <returns>Dados brutos da previsão do tempo</returns>
        /// <response code="200">Previsão obtida com sucesso</response>
        /// <response code="400">Coordenadas inválidas</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("forecast")]
        [ProducesResponseType(typeof(OpenMeteoResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<OpenMeteoResponse>> GetWeatherForecastByCoordinates(
            [FromQuery] decimal latitude,
            [FromQuery] decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                return BadRequest(new { message = "Latitude deve estar entre -90 e 90" });
            }

            if (longitude < -180 || longitude > 180)
            {
                return BadRequest(new { message = "Longitude deve estar entre -180 e 180" });
            }

            try
            {
                var forecast = await _openMeteoService.GetWeatherForecastAsync(latitude, longitude);
                if (forecast == null)
                {
                    return StatusCode(500, new { message = "Erro ao obter dados meteorológicos" });
                }
                return Ok(forecast);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Analisa o risco de calor extremo para dados de previsão
        /// </summary>
        /// <param name="request">Dados de temperatura para análise</param>
        /// <returns>Análise de risco de calor extremo</returns>
        /// <response code="200">Análise realizada com sucesso</response>
        /// <response code="400">Dados de entrada inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost("analyze-heat-risk")]
        [ProducesResponseType(typeof(HeatRiskAnalysisResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<HeatRiskAnalysisResponse> AnalyzeHeatRisk([FromBody] HeatRiskAnalysisRequest request)
        {
            if (request?.TemperatureData == null || !request.TemperatureData.Any())
            {
                return BadRequest(new { message = "Dados de temperatura são obrigatórios" });
            }

            try
            {
                var forecasts = request.TemperatureData.Select(t => new WeatherForecast
                {
                    Date = t.Date,
                    TemperatureMax = t.MaxTemperature,
                    TemperatureMin = t.MinTemperature
                }).ToList();

                var processedForecasts = _heatRiskService.ProcessWeatherForecast(new OpenMeteoResponse
                {
                    Daily = new Daily
                    {
                        Time = forecasts.Select(f => f.Date.ToString("yyyy-MM-dd")).ToList(),
                        Temperature_2m_max = forecasts.Select(f => (double)f.TemperatureMax).ToList(),
                        Temperature_2m_min = forecasts.Select(f => (double)f.TemperatureMin).ToList()
                    }
                });

                var hasExtremeRisk = _heatRiskService.HasExtremeHeatRisk(processedForecasts);

                return Ok(new HeatRiskAnalysisResponse
                {
                    HasExtremeHeatRisk = hasExtremeRisk,
                    Forecasts = processedForecasts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém alertas ativos de calor extremo para todas as cidades
        /// </summary>
        /// <returns>Lista de alertas ativos</returns>
        /// <response code="200">Alertas obtidos com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("alerts")]
        [ProducesResponseType(typeof(List<WeatherAlert>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<WeatherAlert>>> GetActiveAlerts()
        {
            try
            {
                var alerts = await _weatherService.GetActiveAlertsAsync();
                return Ok(alerts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }
    }
}

