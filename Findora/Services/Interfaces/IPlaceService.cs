using Findora.ViewModels.Places;

namespace Findora.Services.Interfaces
{
    public interface IPlaceService
    {
        Task<List<PlaceCardViewModel>> GetLatestPlacesAsync(int count);

        Task<List<PlaceCardViewModel>> SearchPlacesAsync(int? governorateId, int? cityId);
        Task<PlaceDetailsViewModel?> GetDetailsAsync(int placeId);
    }
}