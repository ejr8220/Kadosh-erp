namespace Application.Dtos.Request.Security
{
    public class ForgotPasswordRequestDto
    {
        public string UserCodeOrEmail { get; set; } = string.Empty;
    }
}
