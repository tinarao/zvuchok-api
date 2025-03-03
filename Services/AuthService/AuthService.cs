using api.Db;
using api.Dto;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Slugify;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static api.Dto.Auth;

namespace api.Services.AuthService
{
    public class AuthService(ZvuchokContext context, IConfiguration configuration) : IAuthService
    {
        private readonly ZvuchokContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        public async Task<TokensDTO?> Login(LoginDTO dto)
        {
            var user = await _context.Users.Include(u => u.Credentials).FirstOrDefaultAsync(u => u.NormalizedUsername == Utilities.NormalizeString(dto.Username));
            if (user is null)
            {
                return null;
            }

            var isPasswordValid = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.Credentials.PasswordHash, dto.Password);

            if (isPasswordValid != PasswordVerificationResult.Success)
            {
                return null;
            }

            return new TokensDTO
            {
                AccessToken = GenerateAccessToken(user)
            };
        }

        public async Task<ReturnsDTOWithUser> Register(RegisterDTO dto)
        {
            var duplicate = await _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedUsername == Utilities.NormalizeString(dto.Username) || u.Credentials.Email == dto.Email || (u.Credentials.PhoneNumber != null));

            if (duplicate != null)
            {
                return new ReturnsDTOWithUser
                {
                    StatusCode = 400,
                    Message = "Пользователь с такими данными уже существует"
                };
            }

            var slug = new SlugHelper().GenerateSlug(dto.Username);

            var credentials = new UserCredentials
            {
                PasswordHash = "",
                Email = dto.Email,
            };

            var user = new User
            {
                Username = dto.Username,
                Slug = slug,
                Credentials = credentials,
                NormalizedUsername = Utilities.NormalizeString(dto.Username),
            };

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, dto.Password);

            user.Credentials.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ReturnsDTOWithUser
            {
                Message = "Ok",
                StatusCode = 201,
                User = user,
            };
        }

        public async Task<ReturnsDTOWithUser> Verify(string ctxUsername)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == ctxUsername);
            if (user is null)
            {
                return new ReturnsDTOWithUser
                {
                    StatusCode = 404,
                    Message = "Пользователь не найден"
                };
            }

            return new ReturnsDTOWithUser
            {
                StatusCode = 200,
                User = user,
                Message = "Ok"
            };
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Credentials.Email),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
