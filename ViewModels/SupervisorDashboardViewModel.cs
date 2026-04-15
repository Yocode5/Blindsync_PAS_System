using System.Collections.Generic;

namespace Blindsync_PAS_System.ViewModels 
{
    public class SupervisorDashboardViewModel
    {
        public string FirstName { get; set; }
        
        public List<string> ExpertiseAreas { get; set; } 
        
        public List<string> AvailableResearchAreas { get; set; } 
        
        public List<ProjectProposalViewModel> AlignedProjects { get; set; } 
    }

    public class ProjectProposalViewModel
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string ResearchArea { get; set; }
        public List<string> TechStack { get; set; }
        public string AbstractText { get; set; }
    }
}