using Findora.Models;

namespace Findora.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Governorate>> GetAllGovernoratesAsync();

        Task<List<City>> GetCitiesByGovernorateIdAsync(int governorateId);
        Task<bool> CityBelongsToGovernorateAsync(int cityId, int governorateId);
    }
}