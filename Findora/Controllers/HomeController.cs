using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Findora.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly ILocationService _locationService;

        public HomeController(IPlaceService placeService, ILocationService locationService)
        {
            _placeService = placeService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                Governorates = await _locationService.GetAllGovernoratesAsync(),
                LatestPlaces = await _placeService.GetLatestPlacesAsync(8)
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
