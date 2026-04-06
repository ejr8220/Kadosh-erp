using Domain.Entities.Security;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime expiresAt) CreateAccessToken(User user);
        string CreateRefreshTokenValue();
        string CreatePasswordResetTokenValue();
    }
}
