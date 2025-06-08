using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeatherAlertAPI.Models;
using WeatherAlertAPI.Services;

namespace WeatherAlertAPI.Pages
{
    public class WeatherModel : PageModel
    {
        private readonly ICityService _cityService;
        private readonly IWeatherService _weatherService;

        public WeatherModel(ICityService cityService, IWeatherService weatherService)
        {
            _cityService = cityService;
            _weatherService = weatherService;
        }

        [BindProperty]
        public int? SelectedCityId { get; set; }

        public List<SelectListItem> CityOptions { get; set; } = new();
        public WeatherResponse? WeatherResponse { get; set; }

        public async Task OnGetAsync()
        {
            await LoadCitiesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCitiesAsync();

            if (!SelectedCityId.HasValue)
            {
                ModelState.AddModelError(nameof(SelectedCityId), "Por favor, selecione uma cidade.");
                return Page();
            }

            try
            {
                WeatherResponse = await _weatherService.GetWeatherForecastWithTipsAsync(SelectedCityId.Value);
                
                if (WeatherResponse == null)
                {
                    ModelState.AddModelError("", "Não foi possível obter a previsão do tempo para a cidade selecionada.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro ao buscar previsão do tempo: {ex.Message}");
            }

            return Page();
        }

        private async Task LoadCitiesAsync()
        {
            try
            {
                var cities = await _cityService.GetAllCitiesAsync();
                CityOptions = cities.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nome
                }).ToList();
            }
            catch (Exception)
            {
                CityOptions = new List<SelectListItem>();
            }
        }
    }
}

