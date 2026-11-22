using System.ComponentModel.DataAnnotations;

namespace Findora.ViewModels.Places
{
    public class CreateReviewViewModel
    {
        public int PlaceId { get; set; }


        [Range(1, 5, ErrorMessage = "Please choose a rating.")]
        public int Rating { get; set; }


        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;


        [Required, StringLength(2000)]
        public string Comment { get; set; } = string.Empty;
    }
}