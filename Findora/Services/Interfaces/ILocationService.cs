using Findora.Models;
using Findora.ViewModels.Common;
using Findora.ViewModels.ExplorePage;

namespace Findora.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Governorate>> GetAllGovernoratesAsync();
        Task<List<LookupItemViewModel>> GetGovernoratesAsync();
        Task<List<LookupItemViewModel>> GetCityOptionsAsync(int governorateId);

        Task<List<City>> GetCitiesByGovernorateIdAsync(int governorateId);
        Task<bool> CityBelongsToGovernorateAsync(int cityId, int governorateId);
    }
}