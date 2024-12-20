﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Business.Operations.User;
using Project.Business.Operations.User.Dtos;
using Project.WebApi.Jwt;
using Project.WebApi.Models;

namespace Project.WebApi.Controllers
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

        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addUserDto = new AddUserDto
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Password = registerRequest.Password,
                PhoneNumber = registerRequest.PhoneNumber,
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addAdminDto = new AddAdminDto
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Password = registerRequest.Password,
                PhoneNumber = registerRequest.PhoneNumber,
            };

            var result = await _userService.AddAdmin(addAdminDto);
            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }

        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var loginUserDto = new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password,
            };
            var result = _userService.LoginUser(loginUserDto);
            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }
            var user = result.Data;
            //Property Injection yapıldı.
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });
            return Ok(new LoginResponse
            {
                Message = "Giriş başarıyla gerçekleşti",
                Token = token
            });


        }

        [HttpGet("TokenDeneme")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok();
        }

    }
}
