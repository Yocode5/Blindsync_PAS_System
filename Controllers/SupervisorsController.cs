using Microsoft.AspNetCore.Mvc;
using Blindsync_PAS_System.ViewModels; 
using System.Collections.Generic;

namespace Blindsync_PAS_System.Controllers 
{
    public class SupervisorsController : Controller
    {
        public IActionResult ReviewBoard()
        {
            var expertise = new List<string> { "Machine Learning", "Cloud Computing" };

            var model = new SupervisorDashboardViewModel
            {
                FirstName = "Yasith", 
                ExpertiseAreas = expertise,
                
                AvailableResearchAreas = new List<string> { "Machine Learning", "Cloud Computing", "IoT", "Data Science", "Cyber Security" },
                
                AlignedProjects = new List<ProjectProposalViewModel>
                {
                    new ProjectProposalViewModel 
                    { 
                        ProjectId = 1, 
                        Title = "AI Based Traffic Management", 
                        ResearchArea = "Machine Learning", 
                        TechStack = new List<string> { "Python", "TensorFlow" },
                        AbstractText = "This project aims to optimize city traffic using real-time AI computer vision models."
                    },
                    new ProjectProposalViewModel 
                    { 
                        ProjectId = 2, 
                        Title = "Smart Agriculture IoT", 
                        ResearchArea = "IoT", 
                        TechStack = new List<string> { "C++", "Azure", "React" },
                        AbstractText = "An IoT based system to monitor soil moisture, temperature, and automate irrigation."
                    },
                     new ProjectProposalViewModel 
                    { 
                        ProjectId = 3, 
                        Title = "Scalable Cloud Log Analytics", 
                        ResearchArea = "Cloud Computing", 
                        TechStack = new List<string> { "Go", "AWS", "Grafana" },
                        AbstractText = "A high-performance cloud log analysis platform for microservices."
                    }
                    
                }
            };

            return View(model); 
        }
        public IActionResult MyMatches()
        {
            return View();
        }
        [HttpPost]
public IActionResult AcceptProject(int id)
{
    // Logic: Find the project by ID and assign the supervisor
    // Example: 
    // var project = _context.Projects.Find(id);
    // project.Status = "Accepted";
    // _context.SaveChanges();

    return Ok(); // Return 200 OK to the JavaScript
}
    }
}