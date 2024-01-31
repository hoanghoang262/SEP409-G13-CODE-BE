using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate_Service.Common
{
    public class GenerateJwtToken
    {
        private readonly IConfiguration _configuration;
        public GenerateJwtToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken GenerateToken(int UserId,string email, List<string> userRoles)
        {
            var authClaims = new List<Claim>
            {
                  new Claim(ClaimTypes.Email, email),
                  new Claim(ClaimTypes.Name, UserId.ToString()),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
           

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
