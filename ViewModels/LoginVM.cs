using System.ComponentModel.DataAnnotations;

namespace Blindsync_PAS_System.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@uni\.ac\.lk$", ErrorMessage = "Only @uni.ac.lk emails are allowed")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
