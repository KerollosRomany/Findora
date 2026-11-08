using Findora.Data;
using Findora.Models;
using Findora.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
