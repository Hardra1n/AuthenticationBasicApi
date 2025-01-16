using Microsoft.AspNetCore.Mvc;
using System;
using Application.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthenticationController : Controller
    {
        private SystemUserSource _source;

        public AuthenticationController(SystemUserSource source)
        {
            _source = source;
        }

        [HttpGet]
        [Route("jwt/secret")]
        public IActionResult GetSecret()
        {
            return Json(JwtAuthOptions.Key);
        }

        [HttpGet]
        [Route("auth/ping")]
        [Authorize]
        public IActionResult IsAuthenticated()
        {
            return Ok();
        }

        [HttpPost]
        [Route("jwt/token")]
        public IActionResult GetToken([FromBody] SystemUser user)
        {
            var identity = GetIdentity(user.Username, user.Password);
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
            return Json(encodedJwt);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var systemUser = _source.GetUsers().FirstOrDefault(usr => usr.Username == username && usr.Password == password);
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
