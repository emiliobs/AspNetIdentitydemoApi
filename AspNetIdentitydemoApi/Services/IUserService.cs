using AspNetIdentityDemo.Shared;
using System.Threading.Tasks;

namespace AspNetIdentitydemoApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }
}
