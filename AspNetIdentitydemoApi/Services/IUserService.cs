using AspNetIdentityDemo.Shared;
using System.Threading.Tasks;

namespace AspNetIdentitydemoApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

    }
}
