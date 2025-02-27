using api.Dto;
using static api.Dto.Auth;

namespace api.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ReturnsDTOWithUser> Register(RegisterDTO dto);
        public Task<TokensDTO?> Login(LoginDTO dto);
        public Task<ReturnsDTOWithUser> Verify(string ctxUsername);
    }
}
