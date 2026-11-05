namespace Findora.Models;

public class PlaceAmenity
{
    public int PlaceId { get; set; }
    public Place Place { get; set; }

    public int AmenityId { get; set; }
    public Amenity Amenity { get; set; }
}