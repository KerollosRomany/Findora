using Findora.Models;
using System.ComponentModel.DataAnnotations;

namespace Findora.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        public int GovernorateId { get; set; }
        [Required]
        public int CityId { get; set; }

        public List<Governorate> Governorates { get; set; } = new();
        public List<City> Cities { get; set; } = new();
    }
}