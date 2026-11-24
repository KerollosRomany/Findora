using Findora.Data;
using Findora.Models;
using Findora.Services.Interfaces;
using Findora.ViewModels.ExplorePage;
using Findora.ViewModels.Places;
using Findora.ViewModels.Common;
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
                .ThenBy(p => p.Id)
                .Take(count);

            return await ToCardsAsync(query);
        }


        public async Task<PagedResult<PlaceCardViewModel>> SearchAsync(ExploreFilterViewModel filter, int pageSize)
        {
            var query = _context.Places.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.PlaceName))
            {
                query = query.Where(p => p.Name.Contains(filter.PlaceName));
            }

            if (filter.CityId.HasValue)
                query = query.Where(p => p.CityId == filter.CityId.Value);

            else if (filter.GovernorateId.HasValue)
                query = query.Where(p => p.City.GovernorateId == filter.GovernorateId.Value);

            foreach (var amenityId in filter.SelectedAmenityIds)
            {
                query = query.Where(p => p.PlaceAmenities.Any(pa => pa.AmenityId == amenityId));
            }

            var totalPlaces = await query.CountAsync();

            filter.Page = Math.Max(1, filter.Page);
            var page = filter.Page;

            query = query.OrderByDescending(p => p.CreatedAt)
                         .ThenBy(p => p.Id)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize); var results = await ToCardsAsync(query);

            return new PagedResult<PlaceCardViewModel>
            {
                Items = results,
                TotalCount = totalPlaces
            };
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

        public async Task<PlaceDetailsViewModel?> GetDetailsAsync(int placeId, string? currentUserId)
        {
            var place = await _context.Places
                .AsNoTracking()
                .Where(p => p.Id == placeId)
                .Select(p => new PlaceDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Address = p.Address,
                    Description = p.Description,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    CityName = p.City.Name,
                    GovernorateName = p.City.Governorate.Name,

                    ImageUrls = p.PlaceImages
                        .OrderByDescending(i => i.IsMain)
                        .Select(i => i.ImgURL)
                        .ToList(),

                    Amenities = p.PlaceAmenities
                        .Select(pa => pa.Amenity.Name)
                        .ToList(),

                    Reviews = p.Reviews
                        .OrderByDescending(r => r.CreatedAt)
                        .Select(r => new ReviewViewModel
                        {
                            UserName = r.User.FullName,
                            Rating = r.Rating,
                            Comment = r.Comment,
                            Title = r.Title,
                            CreatedAt = r.CreatedAt
                        })
                        .ToList(),
                 
                    AverageRating = p.Reviews.Average(r => (double?)r.Rating) ?? 0,
                    ReviewsCount = p.Reviews.Count()
                })
                .FirstOrDefaultAsync();

            if (place == null)
                return null;
            if (currentUserId != null)
            {
                place.CurrentUserReviewId = await _context.Reviews
                    .AsNoTracking()
                    .Where(r => r.UserId == currentUserId && r.PlaceId == placeId)
                    .Select(r => (int?)r.Id)
                    .FirstOrDefaultAsync();
            }
            // WorkingHours 
            var hours = await _context.WorkingHours
                .AsNoTracking()
                .Where(h => h.PlaceId  == placeId)
                .ToListAsync();

            var (day, time) = GetEgyptNow();
            var yesterday = (DayOfWeek)(((int)day + 6) % 7);

            place.IsOpenNow = CalculateIsOpenNow(hours, day, yesterday, time);
            place.AverageRating = Math.Round(place.AverageRating, 1);
            // Egypt week order: Saturday first, Friday last
            place.WorkingHours = hours
                .OrderBy(h => ((int)h.Day + 1) % 7)
                .ThenBy(h => h.OpenTime)
                .Select(h => new WorkingHourViewModel
                {
                    Day = h.Day,
                    IsClosed = h.IsClosed,
                    OpenTime = h.OpenTime,
                    CloseTime = h.CloseTime,
                    IsToday = h.Day == day
                })
                .ToList();
            return place;
        }

        public async Task<List<LookupItemViewModel>> GetAmenitiesAsync()
        {
            return await _context.Amenities
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .Select(a => new LookupItemViewModel { Id = a.Id, Name = a.Name })
                .ToListAsync();
        }
    }
}