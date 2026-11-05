using System.ComponentModel.DataAnnotations;

namespace Findora.Models;

public class PlaceImages
{
    public int Id { get; set; }
    public string ImgURL { get; set; }
    public bool IsMain { get; set; }
    public int PlaceId { get; set; }
    public Place Place { get; set; }

}
