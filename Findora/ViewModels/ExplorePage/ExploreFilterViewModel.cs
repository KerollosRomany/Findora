namespace Findora.ViewModels.ExplorePage
{
    public class ExploreFilterViewModel
    {
        public string? PlaceName { get; set; }

        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public List<int> SelectedAmenityIds { get; set; } = new();
        public int Page { get; set; } = 1;
    }
}
