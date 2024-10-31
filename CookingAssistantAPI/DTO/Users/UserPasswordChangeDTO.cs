namespace CookingAssistantAPI.DTO.Users
{
    public class UserPasswordChangeDTO
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? NewPasswordConfirm { get; set; }
    }
}
