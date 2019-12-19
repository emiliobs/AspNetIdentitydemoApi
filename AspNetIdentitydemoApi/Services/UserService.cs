using AspNetIdentityDemo.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitydemoApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Register Model is Null");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn´t match the password.",
                    Issuccess = false,

                };

            }

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created succssfully",
                    Issuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "User did not create.",
                Issuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };

        }
    }
}
