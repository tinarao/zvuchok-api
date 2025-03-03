using api.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static api.Dto.Auth;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpGet("verify"), Authorize]
        public async Task<ActionResult> Verify()
        {
            if (User.Identity == null || User.Identity.Name == null)
            {
                return Unauthorized();
            }

            var result = await _authService.Verify(User.Identity.Name);
            if (result.User is null)
            {
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

            var tokens = await _authService.Login(dto);
            if (tokens is null)
            {
                return Unauthorized("Provided credentials don't match our records");
            }

            return Ok(tokens);
        }
    }
}