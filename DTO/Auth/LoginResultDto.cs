namespace Bulletingboard.DTO.Auth
{
    public class LoginResultDto
    {
        public string Username { get; set; }
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
       
}