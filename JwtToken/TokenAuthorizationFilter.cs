using Knila_Projects.InterfaceRepositories;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Knila_Projects.Encryption;
using System.Text;

namespace Knila_Projects.JwtToken
{
    public class TokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IKnilaTokenRepository _tokenRepository;
        private readonly string _secretKey = "Your256BitLongSecretKeyForHS256Signing"; 

        public TokenAuthorizationFilter(IKnilaTokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Check for the Authorization header
            var request = context.HttpContext.Request;
            if (!request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = tokenHeader.ToString().Replace("Bearer ", "");

            
            var userId = EncryptionHelper.DecryptAndExtractUserId(token, _secretKey);
            if (userId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Convert Base64 string to byte array
            byte[] decodedBytes = Convert.FromBase64String(userId);

            // Decode byte array to string
            string decodedName = Encoding.UTF8.GetString(decodedBytes);

            var storedToken = await _tokenRepository.GetTokenByUserIdAsync(int.Parse(decodedName));
            if (storedToken == null )
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            return;

           
        }
    }
}
