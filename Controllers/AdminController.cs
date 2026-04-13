using Microsoft.AspNetCore.Mvc;
using Blindsync_PAS_System.ViewModels;
using System.Collections.Generic;

namespace Blindsync_PAS_System.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Overview()
        {
            var viewModel = new AdminOverviewVM
            {
                TotalStudents = 300,
                TotalMatched = 200,
                TotalSupervisors = 150,
                TotalPending = 100,
                Projects = new List<ProjectOverviewDto>
                {
                    new ProjectOverviewDto { ProjectId = 1, ProjectTitle = "AI Diagnosis", StudentId = "ST101", StudentName = "John Doe", SupervisorName = "Dr. Smith", Status = "Matched" },
                    new ProjectOverviewDto { ProjectId = 2, ProjectTitle = "Web Scraper", StudentId = "ST102", StudentName = "Jane Roe", SupervisorName = "Pending", Status = "Pending" },
                    new ProjectOverviewDto { ProjectId = 3, ProjectTitle = "Crypto Wallet", StudentId = "ST103", StudentName = "Sam Wilson", SupervisorName = "Dr. Silva", Status = "Under Review" }
                }
            };
            return View(viewModel);
        }
    }
}
