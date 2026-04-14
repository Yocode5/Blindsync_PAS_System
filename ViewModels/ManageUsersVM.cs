namespace Blindsync_PAS_System.ViewModels
{
    public class ManageUsersVM
    {
        public List<StudentVM> Students { get; set; } = new List<StudentVM>();
        public List<SupervisorVM> Supervisors { get; set; } = new List<SupervisorVM>();
        public List<AdminVM> Admins { get; set; } = new List<AdminVM>();
    }

    public class StudentVM
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }

    public class SupervisorVM {
        public int UserId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string SupervisorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int ProjectQuota { get; set; }
        public bool IsActive { get; set; }
    }

    public class AdminVM
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
