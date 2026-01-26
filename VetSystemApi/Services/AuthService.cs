using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Entities;
using Microsoft.Extensions.Options;

namespace VetSystemApi.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly DefaultUserSettings _defaultUserSettings;

        public AuthService(AppDbContext context, IPasswordHasher<User> passwordhasher, IOptions<JwtSettings> jwtOptions, IOptions<DefaultUserSettings> defaultUserOptions)
        {
            _context = context;
            _passwordHasher = passwordhasher;   
            _jwtSettings = jwtOptions.Value;
            _defaultUserSettings = defaultUserOptions.Value;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
            {
                return null!;
            }

            if (user.Role == null)
            {
                throw new InvalidOperationException("User has no role assigned.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? _defaultUserSettings.DefaultRole) 
            };

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires= DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescritor);
            return tokenHandler.WriteToken(token);
        }
    }        
}
