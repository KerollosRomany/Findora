namespace Findora.Models;

public class Favourites
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int PlaceId { get; set; }
    public Place Place { get; set; }
}