using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Blindsync_PAS_System.Models
{
    public enum ProjectStatus
    {
        Pending,
        Matched,
        UnderReview,
        Withdrawn,
    }

    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string TechStack { get; set; } = string.Empty;

        [Required]
        public string Abstract { get; set; }

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student Creator { get; set; }

        [Required]
        public int ResearchAreaId { get; set; }
        [ForeignKey("ResearchAreaId")]
        public ResearchArea Area { get; set; }

        public int? SupervisorId { get; set; }
        [ForeignKey("SupervisorId")]
        public Supervisor? AssignedSupervisor { get; set; }

        public DateTime? AssignedAt { get; set; }
    }
}
