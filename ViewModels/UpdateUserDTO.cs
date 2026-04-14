using System.ComponentModel.DataAnnotations;

namespace Blindsync_PAS_System.ViewModels
{
    public class UpdateUserDTO
    {
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? Password { get; set; }
    }
}
