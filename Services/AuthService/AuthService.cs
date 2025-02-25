using api.Db;
using api.Dto;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Slugify;
using static api.Dto.Auth;

namespace api.Services.AuthService
{
    public class AuthService(ZvuchokContext context) : IAuthService
    {
        private readonly ZvuchokContext _context = context;

        public async Task<ReturnsDTOWithUser> Login(Auth.LoginDTO dto)
        {
            var user = await _context.Users.Include(u => u.Credentials).FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user is null)
            {
                return new ReturnsDTOWithUser
                {
                    StatusCode = 404,
                    Message = "Пользователь с такими данными не найден"
                };
            }

            var isPasswordValid = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.Credentials.PasswordHash, dto.Password);

            if (isPasswordValid != PasswordVerificationResult.Success)
            {
                return new ReturnsDTOWithUser
                {
                    StatusCode = 404,
                    Message = "Пользователь с такими данными не найден"
                };
            }

            return new ReturnsDTOWithUser
            {
                Message = "Пользователь успешно авторизован",
                StatusCode = 200,
                User = user,
            };
        }

        public async Task<ReturnsDTOWithUser> Register(RegisterDTO dto)
        {
            var duplicate = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username || u.Credentials.Email == dto.Email || (u.Credentials.PhoneNumber != null && u.Credentials.PhoneNumber == dto.PhoneNumber));

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
                NormalizedUsername = dto.Username.ToLower(),
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
    }
}
