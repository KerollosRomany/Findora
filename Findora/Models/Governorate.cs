using System.ComponentModel.DataAnnotations;

namespace Findora.Models;

public class Governorate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<City> Cities { get; set; } = new List<City>();
}
