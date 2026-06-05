namespace Bulletingboard.Requests.Auth
{
    public class ResetPasswordRequest
    {
        public int Id { get; set; }
        public string NewPassword { get; set; }=string.Empty;
        public string ConfirmPassword { get; set; }=string.Empty;
    }
}
