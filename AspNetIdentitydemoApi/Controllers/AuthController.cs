﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetIdentityDemo.Shared;
using AspNetIdentitydemoApi.Models;
using AspNetIdentitydemoApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AspNetIdentitydemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IEmailService emailService, IConfiguration configuration)
        {
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
        }


        // /api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.IsSuccess)
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


        // /api/Auth/Login/
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                {

                    await _emailService.SendEmailAsync(model.Email, "New login", "<h1>Hey!, new login to your account noticed</h1><p>New login to your account at " + DateTime.Now + "</p>");
                    //await _emailService.SendEmailAsync(model.Email,"New Login.", "<h1>Hey!, new login yo your account noticed</h1><p>New login to your account at "+ DateTime.Now+" </p>");
                    
                    
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }

                
            }

            return BadRequest("some properties are not Valid.");
        }


         // /api/auth/confirmemail/?userid&token
         [HttpGet("ConfirmEmail")]
         public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }


            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.IsSuccess)
            {
                return Redirect($"{_configuration["appUrl"]}/confirmemail.html");
            }

            return BadRequest(result);
        }

        // /api/auth/ForgetPassword
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.IsSuccess)
            {
                return Ok(result);          //200
            }

            return BadRequest(result); //400
        }

        // api/auth/resetpassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
    }
}