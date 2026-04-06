using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordRequestDtoValidator()
        {
            RuleFor(x => x.UserCodeOrEmail)
                .NotEmpty().WithMessage("Usuario o correo es obligatorio.");
        }
    }
}
