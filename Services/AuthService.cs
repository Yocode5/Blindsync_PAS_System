using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.Models;
using Blindsync_PAS_System.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Blindsync_PAS_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public string ValidateUser(LoginVM model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);

            if (user != null && user.IsActive)
            {
                
                var hasher = new PasswordHasher<User>();

                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                // 3. CHECK FOR SUCCESS
                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return user.Role;
                }
            }

            return null;
        }
    }
}
