using Findora.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var place = await _placeService.GetDetailsAsync(id, userId);
            if (place == null) return NotFound();
            return View(place);
        }
    }
}
