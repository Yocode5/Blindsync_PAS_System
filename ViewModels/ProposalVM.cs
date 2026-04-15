using System.ComponentModel.DataAnnotations;

namespace Blindsync_PAS_System.ViewModels
{
    public class ProposalVM
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Project Title is required.")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a Research Area.")]
        public int? ResearchAreaId { get; set; }

        [Required(ErrorMessage = "Please add at least one technology.")]
        public string TechStack { get; set; }

        [Required(ErrorMessage = "Abstract is required.")]
        public string Abstract { get; set; }
    }
}