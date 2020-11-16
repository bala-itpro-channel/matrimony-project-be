using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Matrimony.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Matrimony.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: ControllerBase
    {

        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string userName, string password)
        {
            IActionResult response = Unauthorized();
            if (userName == "test" || password == "test")
            {
                return BadRequest();
            }

            Profile profile = new Profile
            {
                UserName = userName,
                Password = password,
                Email = "bala@email.com"
            };

            if (profile == null)
            {
                return response;
            }

            string tokenString = GenerateJSONWebToken(profile);
            response = Ok(new { token = tokenString });

            return response;
        }

        private string GenerateJSONWebToken(Profile profile)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, profile.UserName),
                new Claim(JwtRegisteredClaimNames.Email, profile.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

    }
    
}
