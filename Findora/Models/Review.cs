using System.ComponentModel.DataAnnotations;

namespace Findora.Models;

public class Review
{
   public int Id { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    public string Title { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public int PlaceId { get; set; }
    public Place Place { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}