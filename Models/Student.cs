using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blindsync_PAS_System.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string StudentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserAccount { get; set; }
    }
}
