using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Knila_Projects.Encryption
{
    public static class EncryptionHelper
    {
        
        public static string DecryptAndExtractUserId(string token, string secretKey)
        {
            var handler = new JwtSecurityTokenHandler();

           
            if (!(handler.ReadToken(token) is JwtSecurityToken jwtToken))
                return null;

            
            var encryptedUserIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;


             if (encryptedUserIdClaim == null) return null;

             return encryptedUserIdClaim;

        }








        }

}

