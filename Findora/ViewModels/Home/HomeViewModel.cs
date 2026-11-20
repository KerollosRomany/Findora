using Findora.Models;
using Findora.ViewModels.Places;

namespace Findora.ViewModels.Home;

public class HomeViewModel
{
    public int? GovernorateId { get; set; }
    public int? CityId { get; set; }

    public List<Governorate> Governorates { get; set; } = new();
    public List<City> Cities { get; set; } = new();

    public List<PlaceCardViewModel> LatestPlaces { get; set; } = new();
}