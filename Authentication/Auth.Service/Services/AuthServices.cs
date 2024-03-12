using Auth.Data.Entity;
using Auth.Data.Repository;
using Auth.Service.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Service.Services
{

    public interface IAuthService
    {
        Task<string> VerifyPasswordAsync(CredentialDTO dto);
    }
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> VerifyPasswordAsync(CredentialDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.UserName))
                {
                    throw new ArgumentException("Email and password are required.");
                }

                var user = await _userRepository.GetUserByEmailAsync(dto.UserName);

                if (user == null)
                {
                    throw new ArgumentException("User not found.");
                }
                bool isPasswordValid = VerifyHashedPassword(dto.Password, user.PasswordHash, user.Salt);

                if (isPasswordValid)
                {
                    return CreateToken(dto);
                }
                throw new ArgumentException("Invalid username or password");
            }
            catch(Exception) {
                throw;
            }
            
        }

        private bool VerifyHashedPassword(string password, string hashedPassword, string salt)
        {
            return BCrypt.Net.BCrypt.Verify(password + salt,hashedPassword);
        }

        private string CreateToken(CredentialDTO user)
        {
            var issuer = _configuration.GetSection("Jwt:Issuer").Value;
            var audience = _configuration.GetSection("Jwt:Audience").Value;
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Key").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(50),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
