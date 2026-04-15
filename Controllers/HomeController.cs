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
        public IActionResult Profile(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return RedirectToAction("Login", "Home");

            var viewModel = new UserProfileVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileVM model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return RedirectToAction("Login", "Home");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);

                if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success ||
                    result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "The current password you entered is incorrect.");
                    return View(model);
                }
            }

            _context.SaveChanges();

            var oldEmail = User.Identity.Name;
            if (oldEmail != model.Email)
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

            TempData["SuccessMessage"] = "Profile updated successfully!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (user.Role == "Admin") return RedirectToAction("Overview", "Admin");
            if (user.Role == "Student") return RedirectToAction("Dashboard", "Students");
            if (user.Role == "Supervisor") return RedirectToAction("ReviewBoard", "Supervisors");

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}