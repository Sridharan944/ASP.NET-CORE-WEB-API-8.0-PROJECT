using Knila_Projects.ApplicationDbContext;
using Knila_Projects.Encryption;
using Knila_Projects.Entities;
using Knila_Projects.InterfaceRepositories;
using Knila_Projects.JwtToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Knila_Projects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IKnilaTokenRepository _knilaTokenRepository;
        private readonly AppDbContext _dbContext;

        public AuthController(IUserRepository userRepository, AppDbContext dbContext, IConfiguration configuration, IKnilaTokenRepository knilaTokenRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _knilaTokenRepository = knilaTokenRepository;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            try
            {
                
                var user = await _userRepository.GetUserByCredentialsAsync(login.Username, login.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                
                var existingToken = await _knilaTokenRepository.GetLatestTokenByUserIdAsync(user.UserId);
                if (existingToken != null && existingToken.ExpiryTime > DateTime.Now)
                {
                    existingToken.ExpiryTime = DateTime.Now.AddMinutes(30);
                    _dbContext.KnilaTokenTbl.Update(existingToken);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new { Token = existingToken.Token, userid = user.UserId });
                }

                
                string secretKey = "Your256BitLongSecretKeyForHS256Signing";
                string usedId = user.UserId.ToString();

                // Convert string to byte array
                byte[] nameBytes = Encoding.UTF8.GetBytes(usedId);

                // Encode byte array to Base64
                string encodedusedId = Convert.ToBase64String(nameBytes);

                
                var token = JwtHelper.GenerateJwtToken(
                    encodedusedId,
                    secretKey
                );

                var knilaToken = new KnilaToken
                {
                    Token = token,
                    TokenTime = DateTime.Now,
                    ExpiryTime = DateTime.Now.AddMinutes(30),
                    UserId = user.UserId
                };

                await _knilaTokenRepository.AddAsync(knilaToken);

              
                return Ok(new { Token = token, userid = user.UserId });
            }
            catch (Exception ex)
            {
             
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred during the login process: {ex.Message}");
            }
        }


    }
}



//string encryptedIssuer = EncryptionHelper.Encrypt("abvchgsjhskhkidsuyiasjksdjlkjklsdakl465576", secretKey);
//string encryptedAudience = EncryptionHelper.Encrypt("dhdkshfkjiuejmwwwhuunmsmsd256jkkjkk", secretKey);