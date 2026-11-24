using Findora.Models;
using Findora.ViewModels.Common;
using Findora.ViewModels.ExplorePage;
using Findora.ViewModels.Places;

namespace Findora.ViewModels.ExplorePage
{
    public class ExploreViewModel
    {

        public List<LookupItemViewModel> Governorates { get; set; } = new();
        public List<LookupItemViewModel> Cities { get; set; } = new();
        public List <LookupItemViewModel> Amenities { get; set; } = new();

        public List<PlaceCardViewModel> PlaceCards { get; set; } = new();
        public ExploreFilterViewModel Filter { get; set; } = new();

        // Pagination
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    }

}

