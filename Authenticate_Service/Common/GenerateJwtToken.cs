using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate_Service.Common
{
    public class GenerateJwtToken
    {
        public string GenerateToken(string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                // Có thể thêm các claim khác tùy thuộc vào logic ứng dụng
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere")); // Sử dụng một khóa bí mật an toàn
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1); // Thời gian hết hạn của token

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
