using Application.Dtos.Request.Security;
using Application.Dtos.Response.Security;
using Application.Interfaces;
using Domain.Entities.Security;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<CompanyUser> _companyUserRepository;
        private readonly IRepository<PasswordHistory> _passwordHistoryRepository;
        private readonly IRepository<PasswordResetToken> _passwordResetTokenRepository;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<CompanyUser> companyUserRepository,
            IRepository<PasswordHistory> passwordHistoryRepository,
            IRepository<PasswordResetToken> passwordResetTokenRepository,
            IRepository<RefreshToken> refreshTokenRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _companyUserRepository = companyUserRepository;
            _passwordHistoryRepository = passwordHistoryRepository;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<UserResponseDto> RegisterAsync(UserRequestDto request)
        {
            var userCode = request.UserCode.Trim();
            var email = request.Email.Trim();

            var exists = await _userRepository.AnyAsync(x =>
                (x.UserCode == userCode || x.Email == email) && !x.IsDeleted);

            if (exists)
            {
                throw new UserException("El usuario o correo ya existe.");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                UserCode = userCode,
                Email = email,
                PasswordHash = passwordHash,
                LastPasswordChangeAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            foreach (var roleId in request.RoleIds.Distinct())
            {
                await _userRoleRepository.AddAsync(new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                });
            }

            foreach (var companyId in request.CompanyIds.Distinct())
            {
                await _companyUserRepository.AddAsync(new CompanyUser
                {
                    UserId = user.Id,
                    CompanyId = companyId
                });
            }

            await _passwordHistoryRepository.AddAsync(new PasswordHistory
            {
                UserId = user.Id,
                PasswordHash = passwordHash,
                ChangedAt = DateTime.UtcNow
            });

            await _passwordHistoryRepository.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                UserCode = user.UserCode,
                Email = user.Email
            };
        }

        public async Task<AuthTokenResponseDto> LoginAsync(LoginRequestDto request)
        {
            var lookup = request.UserCodeOrEmail.Trim();
            var users = await _userRepository.FindAsync(x =>
                (x.UserCode == lookup || x.Email == lookup) && !x.IsDeleted);

            var user = users.FirstOrDefault();
            if (user is null)
            {
                throw new UserException("Credenciales inválidas.");
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UserException("Credenciales inválidas.");
            }

            var access = _tokenService.CreateAccessToken(user);
            var refreshValue = _tokenService.CreateRefreshTokenValue();
            var refreshExpiry = DateTime.UtcNow.AddDays(7);

            await _refreshTokenRepository.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshValue,
                ExpiresAt = refreshExpiry,
                IsRevoked = false
            });
            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthTokenResponseDto
            {
                AccessToken = access.token,
                AccessTokenExpiresAt = access.expiresAt,
                RefreshToken = refreshValue,
                RefreshTokenExpiresAt = refreshExpiry,
                UserId = user.Id,
                UserCode = user.UserCode
            };
        }

        public async Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var lookup = request.UserCodeOrEmail.Trim();
            var users = await _userRepository.FindAsync(x =>
                (x.UserCode == lookup || x.Email == lookup) && !x.IsDeleted);

            var user = users.FirstOrDefault();
            if (user is null)
            {
                throw new UserException("No existe el usuario solicitado.");
            }

            var tokenValue = _tokenService.CreatePasswordResetTokenValue();
            var expiresAt = DateTime.UtcNow.AddMinutes(30);

            await _passwordResetTokenRepository.AddAsync(new PasswordResetToken
            {
                UserId = user.Id,
                Token = tokenValue,
                ExpiresAt = expiresAt,
                IsUsed = false
            });
            await _passwordResetTokenRepository.SaveChangesAsync();

            return new ForgotPasswordResponseDto
            {
                ResetToken = tokenValue,
                ExpiresAt = expiresAt
            };
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var tokens = await _passwordResetTokenRepository.FindAsync(x =>
                x.Token == request.Token && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow && !x.IsDeleted);
            var token = tokens.FirstOrDefault();

            if (token is null)
            {
                throw new UserException("El token de recuperación no es válido o expiró.");
            }

            var user = await _userRepository.GetByIdAsync(token.UserId);
            if (user is null)
            {
                throw new UserException("Usuario no encontrado.");
            }

            await EnsureNotInRecentHistoryAsync(user.Id, request.NewPassword);

            var newHash = _passwordHasher.HashPassword(request.NewPassword);
            user.PasswordHash = newHash;
            user.LastPasswordChangeAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            token.IsUsed = true;
            token.UsedAt = DateTime.UtcNow;
            await _passwordResetTokenRepository.UpdateAsync(token);

            await _passwordHistoryRepository.AddAsync(new PasswordHistory
            {
                UserId = user.Id,
                PasswordHash = newHash,
                ChangedAt = DateTime.UtcNow
            });

            await _passwordHistoryRepository.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            var lookup = request.UserCodeOrEmail.Trim();
            var users = await _userRepository.FindAsync(x =>
                (x.UserCode == lookup || x.Email == lookup) && !x.IsDeleted);
            var user = users.FirstOrDefault();

            if (user is null)
            {
                throw new UserException("Usuario no encontrado.");
            }

            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                throw new UserException("La contraseña actual no es correcta.");
            }

            await EnsureNotInRecentHistoryAsync(user.Id, request.NewPassword);

            var newHash = _passwordHasher.HashPassword(request.NewPassword);
            user.PasswordHash = newHash;
            user.LastPasswordChangeAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            await _passwordHistoryRepository.AddAsync(new PasswordHistory
            {
                UserId = user.Id,
                PasswordHash = newHash,
                ChangedAt = DateTime.UtcNow
            });

            await _passwordHistoryRepository.SaveChangesAsync();
        }

        private async Task EnsureNotInRecentHistoryAsync(int userId, string password)
        {
            var passwordHistory = await _passwordHistoryRepository.FindAsync(x => x.UserId == userId && !x.IsDeleted);

            var recent = passwordHistory
                .OrderByDescending(x => x.ChangedAt)
                .Take(5)
                .ToList();

            var reused = recent.Any(item => _passwordHasher.VerifyPassword(password, item.PasswordHash));
            if (reused)
            {
                throw new UserException("No puede reutilizar ninguna de las ultimas 5 contraseñas.");
            }
        }
    }
}
