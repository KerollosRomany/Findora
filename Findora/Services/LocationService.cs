using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Findora.Data;

namespace Findora.Services
{
    public class LocationService : ILocationService
    {
        private readonly AppDbContext _context;
        public LocationService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }


        public async Task<List<Governorate>> GetAllGovernoratesAsync()
        {
            return await _context.Governorates
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<City>> GetCitiesByGovernorateIdAsync(int governorateId)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.GovernorateId == governorateId)
                .ToListAsync();
        }
        public async Task<bool> CityBelongsToGovernorateAsync(int cityId, int governorateId)
        {
            return await _context.Cities.AnyAsync(c =>
                c.Id == cityId && c.GovernorateId == governorateId);
        }

        public async Task<List<LookupItemViewModel>> GetGovernoratesAsync()
        {
            return await _context.Governorates
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .Select(a => new LookupItemViewModel { Id = a.Id, Name = a.Name })
                .ToListAsync();
        }
        public async Task<List<LookupItemViewModel>> GetCityOptionsAsync(int governorateId)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.GovernorateId == governorateId)
                .OrderBy(c => c.Name)
                .Select(c => new LookupItemViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();
        }
    }
}
