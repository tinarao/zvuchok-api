using System.Security.Claims;
using api.Db;
using api.Models;
using api.Services.AuthService;
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
    public class AuthController(IAuthService authService) : ControllerBase
    {

        private readonly IAuthService _authService = authService;

        [HttpGet("verify")]
        public async Task<ActionResult> Verify()
        {
            if (HttpContext.User.Identity is null)
            {
                return Unauthorized();
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var username = HttpContext.User.Identity.Name;
            if (username is null)
            {
                return Unauthorized();
            }

            var result = await _authService.Verify(username);
            if (result.User is null)
            {
                await HttpContext.SignOutAsync();
                return Unauthorized();
            }

            return Ok(result.User);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity("Некорректный запрос");
            }

            var result = await _authService.Register(dto);
            if (result.User is null)
            {
                return BadRequest("Пользователь с такими данными уже существует");
            }

            await SignIn(HttpContext, result.User);
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

            var result = await _authService.Login(dto);
            if (result.User is null)
            {
                return NotFound(result.Message);
            }

            await SignIn(HttpContext, result.User);
            return Ok(result.Message);
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