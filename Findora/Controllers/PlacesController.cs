using Findora.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Findora.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;
        public PlacesController(IPlaceService placeService) 
        {
            _placeService = placeService;
        }


        public async Task<IActionResult> Details(int id)
        {
            var place = await _placeService.GetDetailsAsync(id);
            if (place == null) return NotFound();
            return View (place);
        }
    }
}
