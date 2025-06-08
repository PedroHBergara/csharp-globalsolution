using Microsoft.AspNetCore.Mvc;
using WeatherAlertAPI.Models;
using WeatherAlertAPI.Services;

namespace WeatherAlertAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de dicas preventivas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TipsController : ControllerBase
    {
        private readonly ITipsService _tipsService;

        public TipsController(ITipsService tipsService)
        {
            _tipsService = tipsService;
        }

        /// <summary>
        /// Obtém todas as dicas preventivas cadastradas
        /// </summary>
        /// <returns>Lista de dicas preventivas</returns>
        /// <response code="200">Dicas obtidas com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Dica>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Dica>>> GetAllTips()
        {
            try
            {
                var tips = await _tipsService.GetAllTipsAsync();
                return Ok(tips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém dicas preventivas por nível de risco
        /// </summary>
        /// <param name="riskLevel">Nível de risco (normal, alerta, risco)</param>
        /// <returns>Lista de dicas para o nível de risco especificado</returns>
        /// <response code="200">Dicas obtidas com sucesso</response>
        /// <response code="400">Nível de risco inválido</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("by-risk-level/{riskLevel}")]
        [ProducesResponseType(typeof(List<Dica>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Dica>>> GetTipsByRiskLevel(string riskLevel)
        {
            var validRiskLevels = new[] { "normal", "alerta", "risco" };
            if (!validRiskLevels.Contains(riskLevel.ToLower()))
            {
                return BadRequest(new { 
                    message = "Nível de risco inválido", 
                    validLevels = validRiskLevels 
                });
            }

            try
            {
                var tips = await _tipsService.GetTipsByRiskLevelAsync(riskLevel.ToLower());
                return Ok(tips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém dicas preventivas recomendadas baseadas em temperatura
        /// </summary>
        /// <param name="temperature">Temperatura em graus Celsius</param>
        /// <returns>Lista de dicas recomendadas para a temperatura</returns>
        /// <response code="200">Dicas recomendadas obtidas com sucesso</response>
        /// <response code="400">Temperatura inválida</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("recommendations")]
        [ProducesResponseType(typeof(TipsRecommendationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TipsRecommendationResponse>> GetRecommendedTips([FromQuery] decimal temperature)
        {
            if (temperature < -50 || temperature > 60)
            {
                return BadRequest(new { message = "Temperatura deve estar entre -50°C e 60°C" });
            }

            try
            {
                string riskLevel;
                if (temperature >= 35)
                    riskLevel = "risco";
                else if (temperature >= 30)
                    riskLevel = "alerta";
                else
                    riskLevel = "normal";

                var tips = await _tipsService.GetTipsByRiskLevelAsync(riskLevel);
                
                return Ok(new TipsRecommendationResponse
                {
                    Temperature = temperature,
                    RiskLevel = riskLevel,
                    RiskDescription = GetRiskDescription(riskLevel),
                    Tips = tips.Select(t => t.Mensagem ?? string.Empty).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém estatísticas das dicas por nível de risco
        /// </summary>
        /// <returns>Estatísticas das dicas cadastradas</returns>
        /// <response code="200">Estatísticas obtidas com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("statistics")]
        [ProducesResponseType(typeof(TipsStatisticsResponse), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TipsStatisticsResponse>> GetTipsStatistics()
        {
            try
            {
                var allTips = await _tipsService.GetAllTipsAsync();
                
                var statistics = new TipsStatisticsResponse
                {
                    TotalTips = allTips.Count,
                    TipsByRiskLevel = allTips
                        .GroupBy(t => t.NivelRisco ?? "unknown")
                        .ToDictionary(g => g.Key, g => g.Count()),
                    LastUpdated = DateTime.UtcNow
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        private static string GetRiskDescription(string riskLevel)
        {
            return riskLevel switch
            {
                "normal" => "Condições normais de temperatura",
                "alerta" => "Temperatura elevada - atenção redobrada necessária",
                "risco" => "Calor extremo - risco alto para a saúde",
                _ => "Nível de risco desconhecido"
            };
        }
    }
}

