using Microsoft.IdentityModel.Tokens;
using SimplyFly_Project.DTO;
using SimplyFly_Project.Interfaces;
using SimplyFly_Project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimplyFly_Project.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _keyString;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public TokenService(IConfiguration configuration)
        {
            _keyString = configuration["SecretKey"].ToString();
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyString));
        }
        public async Task<string> GenerateToken(LoginUserDTO user)
        {
            string token = string.Empty;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var cred = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var myToken = tokenHandler.CreateToken(tokenDescription);
            token = tokenHandler.WriteToken(myToken);
            return token;
        }
    }
}
