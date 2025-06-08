using Microsoft.EntityFrameworkCore;
using WeatherAlertAPI.Data;
using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Services
{
    public class TipsService : ITipsService
    {
        private readonly WeatherDbContext _context;
        private readonly ILogger<TipsService> _logger;

        public TipsService(WeatherDbContext context, ILogger<TipsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Dica>> GetAllTipsAsync()
        {
            try
            {
                return await _context.Dicas.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as dicas");
                return new List<Dica>();
            }
        }

        public async Task<List<Dica>> GetTipsByRiskLevelAsync(string riskLevel)
        {
            try
            {
                return await _context.Dicas
                    .Where(d => d.NivelRisco.ToLower() == riskLevel.ToLower())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dicas por nível de risco: {RiskLevel}", riskLevel);
                return new List<Dica>();
            }
        }
    }
}

