namespace Bulletingboard.Requests.User
{
    public class ChangePasswordRequest
    {
        public int Id { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        public void ClearAllField()
        { 
            OldPassword=string.Empty;
            NewPassword=string.Empty;
            ConfirmPassword=string.Empty;
        }
    }
}
