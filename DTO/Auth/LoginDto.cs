using System.ComponentModel.DataAnnotations;

namespace Bulletingboard.DTO.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
