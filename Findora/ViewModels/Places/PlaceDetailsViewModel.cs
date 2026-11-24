using Findora.Enums;

namespace Findora.ViewModels.Places
{
    public class PlaceDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public PlaceType Type { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string GovernorateName { get; set; } = string.Empty;

        public List<string> ImageUrls { get; set; } = new();
        public List<string> Amenities { get; set; } = new();
        public List<WorkingHourViewModel> WorkingHours { get; set; } = new();
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public int? CurrentUserReviewId { get; set; }
        public bool? IsOpenNow { get; set; }
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
    }

    public class WorkingHourViewModel
    {
        public DayOfWeek Day { get; set; }
        public bool IsClosed { get; set; }
        public TimeOnly? OpenTime { get; set; }
        public TimeOnly? CloseTime { get; set; }
        public bool IsToday { get; set; }
    }

    public class ReviewViewModel
    {
        public int Id {  get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title {  get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}