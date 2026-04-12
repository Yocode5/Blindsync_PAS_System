using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blindsync_PAS_System.Models
{
    public class Supervisor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string SupervisorId { get; set; }

        [Required]
        public int ProjectQuota { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserAccount { get; set; }
    }
}
