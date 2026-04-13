using Blindsync_PAS_System.ViewModels;

namespace Blindsync_PAS_System.Services
{
    public interface IAuthService
    {
        string ValidateUser(LoginVM model);
    }
}
