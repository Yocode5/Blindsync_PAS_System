using System.Diagnostics;
using Blindsync_PAS_System.Services;
using Blindsync_PAS_System.ViewModels;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization; 

namespace Blindsync_PAS_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthService _authService;
        private readonly AppDbContext _context; 
        public HomeController(ILogger<HomeController> logger, IAuthService authService, AppDbContext context)
        {
            _logger = logger;
            _authService = authService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userRole = _authService.ValidateUser(model);

            if (userRole != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.Role, userRole)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                TempData["LoginSuccess"] = $"Welcome back!, Logged in as {userRole}.";

                if (userRole == "Admin") return RedirectToAction("Overview", "Admin");
                if (userRole == "Student") return RedirectToAction("Dashboard", "Students");
                if (userRole == "Supervisor") return RedirectToAction("ReviewBoard", "Supervisors");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult LoadProfileModal()
        {
            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return Unauthorized();

            var viewModel = new UserProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return PartialView("_ProfileModal", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("\n", errors) });
            }

            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return Json(new { success = false, message = "User not found." });

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword))
                {
                    return Json(new { success = false, message = "Current password is required to set a new password." });
                }

                var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);

                if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success ||
                    result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
                }
                else
                {
                    return Json(new { success = false, message = "The current password you entered is incorrect." });
                }
            }

            _context.SaveChanges();

            if (userEmail != model.Email)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }

            return Json(new { success = true, message = "Profile updated successfully!" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}