using System.ComponentModel.DataAnnotations;

namespace Blindsync_PAS_System.ViewModels
{
    public class UserProfileVM
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }
    }
}