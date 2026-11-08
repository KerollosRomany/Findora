using Findora.Enums;
using System.ComponentModel.DataAnnotations;

namespace Findora.Models;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PlaceType Type { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }

    [Range(-90, 90)]
    public decimal? Latitude { get; set; }

    [Range(-180, 180)]
    public decimal? Longitude { get; set; }

    public ICollection<PlaceImage> PlaceImages { get; set; } = new List<PlaceImage>();
    public ICollection<PlaceWorkingHour> WorkingHours { get; set; } = new List<PlaceWorkingHour>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<PlaceAmenity> PlaceAmenities { get; set; } = new List<PlaceAmenity>();
    public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
    public int CityId { get; set; }
    public City City { get; set; }

}
