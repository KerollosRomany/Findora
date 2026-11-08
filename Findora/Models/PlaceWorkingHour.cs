using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Findora.Models;

public class PlaceWorkingHour
{
    public int Id { get; set; }
    public DayOfWeek Day { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; } = false;

    public int PlaceId { get; set; }
    public Place Place { get; set; }
}