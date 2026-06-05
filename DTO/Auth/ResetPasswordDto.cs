using Bulletingboard.Requests.Auth;

namespace Bulletingboard.DTO.Auth
{
    public class ResetPasswordDto
    {
        public int UserId {  get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ResetPasswordDto(ResetPasswordRequest resetPasswordRequest)
        {
            UserId = resetPasswordRequest.Id;
            NewPassword = resetPasswordRequest.NewPassword;
            ConfirmPassword = resetPasswordRequest.ConfirmPassword;
        }
    }
}
