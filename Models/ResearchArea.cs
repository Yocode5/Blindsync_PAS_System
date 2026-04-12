using System.ComponentModel.DataAnnotations;

namespace Blindsync_PAS_System.Models
{
    public class ResearchArea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
       
        public bool IsActive { get; set; } = true;
    }
}
