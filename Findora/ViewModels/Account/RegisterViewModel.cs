using Findora.Models;
using System.ComponentModel.DataAnnotations;

namespace Findora.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required, StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public int GovernorateId { get; set; }
        [Required]
        public int CityId { get; set; }

        public List<Governorate> Governorates { get; set; } = new();
        public List<City> Cities { get; set; } = new();
    }
}