using System.Security.Claims;
using api.Db;
using api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Slugify;
using static api.Dto.Auth;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(ZvuchokContext context) : ControllerBase
    {

        private readonly ZvuchokContext _context = context;

        [HttpGet("verify")]
        public async Task<ActionResult> Verify()
        {
            var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated || HttpContext.User.Identity == null)
            {
                return Unauthorized();
            }

            var username = HttpContext.User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null)
            {
                await HttpContext.SignOutAsync();
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var duplicate = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username || u.Credentials.Email == dto.Email || (u.Credentials.PhoneNumber != null && u.Credentials.PhoneNumber == dto.PhoneNumber));

            if (duplicate != null)
            {
                return BadRequest("Пользователь с такими данными уже существует!");
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
                Credentials = credentials
            };

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, dto.Password);

            user.Credentials.PasswordHash = hashedPassword;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignIn(HttpContext, user);

            return Created();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity("Invalid request");
            }

            var isAuthenticated = HttpContext.User.Identity?.IsAuthenticated;
            if (isAuthenticated == true)
            {
                return BadRequest("User is already authenticated");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user is null)
            {
                return NotFound("User with provided credentials is not found");
            }

            var isPasswordValid = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.Credentials.PasswordHash, dto.Password);

            if (isPasswordValid != PasswordVerificationResult.Success)
            {
                return NotFound("User with provided credentials is not found");
            }

            await SignIn(HttpContext, user);

            return Ok("Success");
        }

        /// <summary>
        /// Signs the user in, given the HttpContext and User to sign in
        /// </summary>
        /// <param name="ctx">The HttpContext to sign in</param>
        /// <param name="user">The User to sign in</param>
        /// <returns>Always return true - this way you can await this thing to run</returns>
        private async Task<bool> SignIn(HttpContext ctx, User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return true;
        }
    }
}