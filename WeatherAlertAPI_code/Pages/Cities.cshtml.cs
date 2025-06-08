using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherAlertAPI.Models;
using WeatherAlertAPI.Services;

namespace WeatherAlertAPI.Pages
{
    public class CitiesModel : PageModel
    {
        private readonly ICityService _cityService;

        public CitiesModel(ICityService cityService)
        {
            _cityService = cityService;
        }

        public List<Cidade> Cities { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Cities = await _cityService.GetAllCitiesAsync();
            }
            catch (Exception)
            {
                Cities = new List<Cidade>();
            }
        }
    }
}

