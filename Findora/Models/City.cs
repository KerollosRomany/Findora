using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Findora.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GovernorateId { get; set; }
    public Governorate Governorate { get; set; }
    public ICollection<Place> Places { get; set; } = new List<Place>();

}
