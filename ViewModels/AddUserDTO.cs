using System.ComponentModel.DataAnnotations;

namespace Blindsync_PAS_System.ViewModels
{
    public class AddUserDTO
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }

        public string? StudentId {  get; set; }
        
        public string? SupervisorId { get; set; }

        public int? ProjectQuota { get; set; }
    }
}
