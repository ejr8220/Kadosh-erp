namespace Application.Dtos.Request.Security
{
    public class LoginRequestDto
    {
        public string UserCodeOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
