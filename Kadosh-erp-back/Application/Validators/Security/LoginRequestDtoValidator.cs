using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.UserCodeOrEmail)
                .NotEmpty().WithMessage("Usuario o correo es obligatorio.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");
        }
    }
}
