using AhCh.Users.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AhCh.Users
{
    public static class JwtHelper
    {
        public static string Generate(User user, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Security:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Username),
                new Claim(JwtRegisteredClaimNames.Sid,user.Id.ToString()),
                new Claim("DateOfJoining",user.DateOfJoining.ToString("yyyy-MM-dd")),
            };

            var token = new JwtSecurityToken(configuration["Security:issuer"]!, configuration["Security:issuer"]!, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}