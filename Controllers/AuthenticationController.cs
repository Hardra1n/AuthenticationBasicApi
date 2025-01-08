using Microsoft.AspNetCore.Mvc;
using System;
using Application.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Application.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthenticationController : Controller
    {
        private List<SystemUser> _users = new List<SystemUser>
        {
            new SystemUser {Username="admin@gmail.com", Password="12345"},
            new SystemUser { Username="qwerty@gmail.com", Password="55555"}
        };

        [HttpGet]
        [Route("token")]
        public IActionResult GetToken(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest("Invalid username or password");
            }

            DateTime now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: JwtAuthOptions.Issuer, 
                audience: JwtAuthOptions.Audience, 
                notBefore: now, 
                claims:identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtAuthOptions.Lifetime)),
                signingCredentials: new(JwtAuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new
            {
                Token = encodedJwt,
                Username = username
            };
            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var systemUser = _users.FirstOrDefault(usr => usr.Username == username && usr.Password == password);
            if (systemUser == null)
            {
                return null;
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Token", ClaimTypes.Name, ClaimsIdentity.DefaultRoleClaimType);
            return identity;
        }
    }
}
