using System.ComponentModel.DataAnnotations;

namespace CoffeeHouse.ViewModels
{
    public class PasswordViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}