using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestDtoValidator()
        {
            RuleFor(x => x.UserCodeOrEmail)
                .NotEmpty().WithMessage("Usuario o correo es obligatorio.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("La contraseña actual es obligatoria.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().MinimumLength(8)
                .WithMessage("La nueva contraseña debe tener al menos 8 caracteres.");
        }
    }
}
