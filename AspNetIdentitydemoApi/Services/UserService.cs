using AspNetIdentityDemo.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace AspNetIdentitydemoApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {



            return null;
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that Email address.",
                    Issuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user,model.Password);

            if (!result)
            {
                return new UserManagerResponse 
                {
                      Message = "Invalid Password.",
                      Issuccess = false,
                };
            }

            var claims = new[]
                {
                   new  Claim("Email", model.Email),
                  new Claim(ClaimTypes.NameIdentifier, user.Id),
                };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSetting:Key"]));

            var token = new JwtSecurityToken(

                    issuer: _configuration["AuthSetting:Issuer"],
                    audience: _configuration["AuthSetting:Audince"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: new  SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                  );

            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse 
            { 
                  Message = tokenAsString ,
                  Issuccess = true,
                  ExpireDate = token.ValidTo,
            };



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

                var confirmEmailToken = _userManager.GenerateEmailConfirmationTokenAsync(identityUser);  



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
