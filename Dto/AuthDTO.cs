using System.ComponentModel.DataAnnotations;

namespace api.Dto
{
    public class Auth
    {
        public class TokensDTO
        {
            public required string AccessToken { get; set; }
        }

        public class LoginDTO
        {
            [Required]
            [StringLength(96, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 96 characters long")]
            public required string Username { get; set; }

            [Required]
            [StringLength(96, MinimumLength = 8, ErrorMessage = "Password must be between 2 and 96 characters long")]
            public required string Password { get; set; }
        }

        public class RegisterDTO
        {
            [Required]
            [StringLength(96, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 96 characters long")]
            public required string Username { get; set; }

            [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты!")]
            [StringLength(96, MinimumLength = 8, ErrorMessage = "Email must be between 8 and 96 characters long")]
            public required string Email { get; set; }

            [Required]
            [StringLength(96, MinimumLength = 8, ErrorMessage = "Password must be between 2 and 96 characters long")]
            public required string Password { get; set; }
        }
    }
}