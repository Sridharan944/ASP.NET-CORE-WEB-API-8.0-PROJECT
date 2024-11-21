using Knila_Projects.Encryption;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Knila_Projects.JwtToken
{
    public class JwtHelper
    {
        
        public static string GenerateJwtToken(string userId, string secretKey)
        {
            
            //string encryptedUserId = EncryptionHelper.Encrypt(userId.ToString(), secretKey);

            string encryptedUserId = userId.ToString(); 

            var claims = new[]
            {
            new Claim("UserId", encryptedUserId), 
            
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }








    }
}
