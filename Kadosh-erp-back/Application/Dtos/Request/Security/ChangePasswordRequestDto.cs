namespace Application.Dtos.Request.Security
{
    public class ChangePasswordRequestDto
    {
        public string UserCodeOrEmail { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
