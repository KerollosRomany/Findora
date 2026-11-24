using Findora.Data;
using Findora.Services.Interfaces;
using Findora.ViewModels.Common;
using Findora.ViewModels.ExplorePage;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Findora.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IPlaceService _placeService;
        private readonly ILocationService _locationService;
        private const int PageSize = 9;

        public PlacesController(IPlaceService placeService, ILocationService locationService) 
        {
            _placeService = placeService;
            _locationService = locationService;
        }
        public async Task<IActionResult> Index(ExploreFilterViewModel filter)
        {
            var result = await _placeService.SearchAsync(filter, PageSize);
            var model = new ExploreViewModel
            {
                Governorates = await _locationService.GetGovernoratesAsync(),
                Cities = filter.GovernorateId.HasValue
                        ? await _locationService.GetCityOptionsAsync(filter.GovernorateId.Value) 
                        : new List<LookupItemViewModel>(),
                Amenities = await _placeService.GetAmenitiesAsync(),
                PageSize = PageSize,
                Filter = filter,
                PlaceCards = result.Items,
                TotalCount = result.TotalCount
            };
            return View(model);
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
