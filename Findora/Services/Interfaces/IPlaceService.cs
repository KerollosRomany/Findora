using Findora.ViewModels.ExplorePage;
using Findora.ViewModels.Places;
using Findora.ViewModels.Common;

namespace Findora.Services.Interfaces
{
    public interface IPlaceService
    {
        Task<List<PlaceCardViewModel>> GetLatestPlacesAsync(int count);

        Task<PlaceDetailsViewModel?> GetDetailsAsync(int placeId, string? currentUserId);

        Task<PagedResult<PlaceCardViewModel>> SearchAsync(ExploreFilterViewModel exploreFilterViewModel, int pageSize);
        Task<List<LookupItemViewModel>> GetAmenitiesAsync();
    }
}