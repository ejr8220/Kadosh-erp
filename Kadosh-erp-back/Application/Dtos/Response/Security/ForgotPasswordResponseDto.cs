namespace Application.Dtos.Response.Security
{
    public class ForgotPasswordResponseDto
    {
        public string ResetToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
