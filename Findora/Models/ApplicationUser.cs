using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Findora.Models;
public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // User's location
    public int? GovernorateId { get; set; }
    public Governorate? Governorate { get; set; }

    public int? CityId { get; set; }
    public City? City { get; set;}

    // User relationships
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Favourite> Favorites { get; set; } = new List<Favourite>();

    public ICollection<PlaceSuggestion> PlaceSuggestions { get; set; } = new List<PlaceSuggestion>();

}