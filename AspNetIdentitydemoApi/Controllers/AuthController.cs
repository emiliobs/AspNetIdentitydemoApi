using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetIdentityDemo.Shared;
using AspNetIdentitydemoApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetIdentitydemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        // /api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.Issuccess)
                {
                    return Ok(result);// Satus code: 200s
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest("some properties are not valid.");//Status code : 400
        }

    }
}