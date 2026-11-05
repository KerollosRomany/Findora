using Findora.Enums;

namespace Findora.Models;

public class PlaceSuggestion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PlaceType Type { get; set; }
    public string Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public PlaceSuggestionStatus Status { get; set; } = PlaceSuggestionStatus.Pending;


    public int CityId { get; set; }
    public City City { get; set; }


    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}