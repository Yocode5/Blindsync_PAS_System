namespace Blindsync_PAS_System.ViewModels
{
    public class AdminOverviewVM
    {
        public int TotalStudents { get; set; }
        public int TotalMatched { get; set; }
        public int TotalSupervisors { get; set; }
        public int TotalPending { get; set; }

        public List<ProjectOverviewDto> Projects { get; set; } = new List<ProjectOverviewDto>();
    }

    public class ProjectOverviewDto
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string SupervisorName { get; set; } 
        public string Status { get; set; } 
    }
}