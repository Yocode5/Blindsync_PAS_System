using Blindsync_PAS_System.Data;
using Blindsync_PAS_System.ViewModels;

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
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);

            if (user != null && user.IsActive)
            {
                return user.Role;
            }

            return null;
        }
    }
}
