using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class ResetPasswordRequestDtoValidator : AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("El token es obligatorio.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().MinimumLength(8)
                .WithMessage("La nueva contraseña debe tener al menos 8 caracteres.");
        }
    }
}
