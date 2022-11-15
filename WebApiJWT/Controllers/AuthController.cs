using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiJWT.Constants;
using WebApiJWT.Models;
using WebApiJWT.Repositories;

namespace WebApiJWT.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.User
                .FirstOrDefault(x => x.UserName == request.UserName
                                     && x.Password == request.Password);

            if (user == null)
                throw new BadHttpRequestException("User not found.");

            var accessToken = GenerateAccessToken(user.Id, user.RoleId.GetValueOrDefault());

            var response = new LoginResponse(accessToken, user.Id, user.RoleId, user.UserName);

            return Ok(response);
        }


        private string GenerateAccessToken(int userId, int roleId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConst.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JwtConst.Issuer,
                Audience = JwtConst.Audience,
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtConst.UserId, userId.ToString()),
                new Claim(JwtConst.RoleId, roleId.ToString())
            }),
                Expires = DateTime.Now.AddMinutes(JwtConst.ExpiryMinutes),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
