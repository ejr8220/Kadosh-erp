using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthTokenResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserResponseDto> RegisterAsync(UserRequestDto request);
        Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task ResetPasswordAsync(ResetPasswordRequestDto request);
        Task ChangePasswordAsync(ChangePasswordRequestDto request);
    }
}
