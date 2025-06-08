using Microsoft.AspNetCore.Mvc;
using WeatherAlertAPI.Models;
using WeatherAlertAPI.Services;

namespace WeatherAlertAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de cidades
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        /// <summary>
        /// Obtém todas as cidades cadastradas no sistema
        /// </summary>
        /// <returns>Lista de cidades</returns>
        /// <response code="200">Lista de cidades retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Cidade>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Cidade>>> GetAllCities()
        {
            try
            {
                var cities = await _cityService.GetAllCitiesAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém uma cidade específica por ID
        /// </summary>
        /// <param name="id">ID da cidade</param>
        /// <returns>Dados da cidade</returns>
        /// <response code="200">Cidade encontrada</response>
        /// <response code="404">Cidade não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cidade), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Cidade>> GetCityById(int id)
        {
            try
            {
                var city = await _cityService.GetCityByIdAsync(id);
                if (city == null)
                {
                    return NotFound(new { message = "Cidade não encontrada" });
                }
                return Ok(city);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }

        /// <summary>
        /// Busca cidades por nome
        /// </summary>
        /// <param name="name">Nome da cidade para busca</param>
        /// <returns>Lista de cidades que correspondem ao nome</returns>
        /// <response code="200">Busca realizada com sucesso</response>
        /// <response code="400">Parâmetro de busca inválido</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<Cidade>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<Cidade>>> SearchCitiesByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Nome da cidade é obrigatório" });
            }

            try
            {
                var cities = await _cityService.SearchCitiesByNameAsync(name);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", error = ex.Message });
            }
        }
    }
}

