using Microsoft.EntityFrameworkCore;
using WeatherAlertAPI.Data;
using WeatherAlertAPI.Models;

namespace WeatherAlertAPI.Services
{
    public interface ICityService
    {
        Task<List<Cidade>> GetAllCitiesAsync();
        Task<Cidade?> GetCityByIdAsync(int id);
        Task<List<Cidade>> SearchCitiesByNameAsync(string name);
        Task<Cidade?> CreateCityAsync(Cidade cidade);
        Task<bool> DeleteCityAsync(int id);
    }

    public class CityService : ICityService
    {
        private readonly WeatherDbContext _context;
        private readonly ILogger<CityService> _logger;

        public CityService(WeatherDbContext context, ILogger<CityService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Cidade>> GetAllCitiesAsync()
        {
            try
            {
                return await _context.Cidades.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as cidades");
                return new List<Cidade>();
            }
        }

        public async Task<Cidade?> GetCityByIdAsync(int id)
        {
            try
            {
                return await _context.Cidades.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cidade com ID: {Id}", id);
                return null;
            }
        }

        public async Task<List<Cidade>> SearchCitiesByNameAsync(string name)
        {
            try
            {
                return await _context.Cidades
                    .Where(c => c.Nome.Contains(name))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cidades por nome: {Nome}", name);
                return new List<Cidade>();
            }
        }

        public async Task<Cidade?> CreateCityAsync(Cidade cidade)
        {
            try
            {
                _context.Cidades.Add(cidade);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cidade criada com sucesso. ID: {Id}, Nome: {Nome}", cidade.Id, cidade.Nome);
                return cidade;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cidade: {Nome}", cidade.Nome);
                return null;
            }
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            try
            {
                var cidade = await _context.Cidades.FindAsync(id);
                if (cidade == null)
                {
                    return false;
                }

                _context.Cidades.Remove(cidade);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cidade removida com sucesso. ID: {Id}, Nome: {Nome}", id, cidade.Nome);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover cidade com ID: {Id}", id);
                return false;
            }
        }
    }
}

