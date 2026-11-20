using Findora.Data;
using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.Places;
using Microsoft.EntityFrameworkCore;

namespace Findora.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly AppDbContext _context;

        public PlaceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlaceCardViewModel>> GetLatestPlacesAsync(int count)
        {
            var query = _context.Places
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedAt)
                .Take(count);

            return await ToCardsAsync(query);
        }

        public async Task<List<PlaceCardViewModel>> SearchPlacesAsync(int? governorateId, int? cityId)
        {
            var query = _context.Places.AsNoTracking();

            // City wins if both are sent
            if (cityId.HasValue)
                query = query.Where(p => p.CityId == cityId.Value);
            else if (governorateId.HasValue)
                query = query.Where(p => p.City.GovernorateId == governorateId.Value);

            query = query.OrderByDescending(p => p.CreatedAt);

            return await ToCardsAsync(query);
        }

        private async Task<List<PlaceCardViewModel>> ToCardsAsync(IQueryable<Place> query)
        {
            var (day, time) = GetEgyptNow();
            var yesterday = (DayOfWeek)(((int)day + 6) % 7);

            var rows = await query
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Type,
                    CityName = p.City.Name,
                    GovernorateName = p.City.Governorate.Name,
                    MainImageUrl = p.PlaceImages
                        .Where(i => i.IsMain)
                        .Select(i => i.ImgURL)
                        .FirstOrDefault(),
                    AverageRating = p.Reviews.Average(r => (double?)r.Rating) ?? 0,
                    // Today + yesterday: a shift that passes midnight belongs to yesterday's row
                    Hours = p.WorkingHours
                        .Where(w => w.Day == day || w.Day == yesterday)
                        .ToList()
                })
                .ToListAsync();

            return rows.Select(r => new PlaceCardViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Type = r.Type,
                CityName = r.CityName,
                GovernorateName = r.GovernorateName,
                MainImageUrl = r.MainImageUrl,
                AverageRating = Math.Round(r.AverageRating, 1),
                IsOpenNow = CalculateIsOpenNow(r.Hours, day, yesterday, time)
            }).ToList();
        }

        // true = open, false = closed, null = no working hours saved for today
        private static bool? CalculateIsOpenNow(
            List<PlaceWorkingHour> hours, DayOfWeek today, DayOfWeek yesterday, TimeOnly time)
        {
            var todayRows = hours.Where(h => h.Day == today).ToList();
            var yesterdayRows = hours.Where(h => h.Day == yesterday).ToList();

            // Still inside yesterday's overnight shift (e.g. Sunday 18:00 -> 02:00, now Monday 01:00)
            bool openFromYesterday = yesterdayRows.Any(h =>
                IsValid(h) && h.CloseTime <= h.OpenTime && time < h.CloseTime);
            if (openFromYesterday) return true;

            // No working hours saved for today
            if (todayRows.Count == 0) return null;

            // Any shift today (one, two or more)
            return todayRows.Any(h =>
                IsValid(h) &&
                (h.OpenTime < h.CloseTime
                    ? h.OpenTime <= time && time < h.CloseTime   // normal shift (10:00 -> 14:00)
                    : time >= h.OpenTime));                      // overnight shift, until midnight (18:00 -> 02:00)
        }

        private static bool IsValid(PlaceWorkingHour h) =>
            !h.IsClosed && h.OpenTime.HasValue && h.CloseTime.HasValue;

        private static (DayOfWeek day, TimeOnly time) GetEgyptNow()
        {
            TimeZoneInfo tz;
            try { tz = TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo"); }
            catch (TimeZoneNotFoundException) { tz = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"); }

            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            return (now.DayOfWeek, TimeOnly.FromDateTime(now));
        }
    }
}