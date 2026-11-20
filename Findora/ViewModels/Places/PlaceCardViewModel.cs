using Findora.Enums;

namespace Findora.ViewModels.Places
{
    public class PlaceCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PlaceType Type { get; set; }
        public string CityName { get; set; }
        public string GovernorateName { get; set; }
        public string MainImageUrl { get; set; }
        public bool? IsOpenNow { get; set; }
        public double AverageRating { get; set; }

    }
}
