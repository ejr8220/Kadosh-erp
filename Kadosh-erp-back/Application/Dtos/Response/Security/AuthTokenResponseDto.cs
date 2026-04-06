namespace Application.Dtos.Response.Security
{
    public class AuthTokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
        public int UserId { get; set; }
        public string UserCode { get; set; } = string.Empty;
    }
}
