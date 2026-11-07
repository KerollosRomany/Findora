using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Findora.Models;
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // User's location
    public int? GovernorateId { get; set; }
    public Governorate? Governorate { get; set; }

    public int? CityId { get; set; }
    public City? City { get; set;}

    // User relationships
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Favourites> Favorites { get; set; } = new List<Favourites>();

    public ICollection<PlaceSuggestion> PlaceSuggestions { get; set; } = new List<PlaceSuggestion>();

}