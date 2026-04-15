using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlindSync.Controllers
{
    public class DashboardController : Controller
    {
        // GET: /Dashboard/Index
        public IActionResult Index()
        {
            // Set supervisor name (in real app, get from logged-in user)
            ViewBag.SupervisorName = "Dinula";
            
            // Get projects from service/database
            var projects = GetProjects();
            
            return View(projects);
        }
        
        // POST: /Dashboard/ExpressInterest
        [HttpPost]
        public JsonResult ExpressInterest(int projectId, string abstractText)
        {
            // In production: save to database, notify student, etc.
            // This is where you'd store the supervisor's interest
            
            bool success = true;
            string message = $"Interest recorded for project ID: {projectId}";
            
            
            return Json(new { success = true, message = message });
        }
        
        // Helper method to get project data
        private List<ProjectViewModel> GetProjects()
        {
            // In production: fetch from database
            return new List<ProjectViewModel>
            {
                new ProjectViewModel
                {
                    Id = 1,
                    Name = "AI-Powered Code Assistant",
                    ResearchArea = "Artificial Intelligence",
                    TechStack = new List<string> { "Python", "TensorFlow" },
                    Abstract = "This research explores integrating large language models into developer tooling to assist with real-time code generation, debugging, and documentation."
                },
                new ProjectViewModel
                {
                    Id = 2,
                    Name = "Decentralized Identity Management",
                    ResearchArea = "Web Development",
                    TechStack = new List<string> { "React", "Solidity" },
                    Abstract = "A blockchain-based identity framework that gives users control over personal data using zero-knowledge proofs."
                },
                new ProjectViewModel
                {
                    Id = 3,
                    Name = "Zero-Trust Network Security",
                    ResearchArea = "Cybersecurity",
                    TechStack = new List<string> { "Go", "Kubernetes" },
                    Abstract = "Implementation of micro-segmentation and continuous verification for cloud-native environments."
                }
            };
        }
    }
    
    // View Model for Project
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResearchArea { get; set; }
        public List<string> TechStack { get; set; }
        public string Abstract { get; set; }
    }
}