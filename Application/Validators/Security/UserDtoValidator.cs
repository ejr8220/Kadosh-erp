using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class UserDtoValidator : AbstractValidator<UserRequestDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.UserCode)
                .NotEmpty().WithMessage("El código de usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El código de usuario no puede superar los 50 caracteres.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("Debe proporcionar un correo electrónico válido.")
                .MaximumLength(100).WithMessage("El correo electrónico no puede superar los 100 caracteres.");

            RuleForEach(u => u.RoleIds)
                .GreaterThan(0).WithMessage("Debe seleccionar roles válidos.");

            RuleForEach(u => u.CompanyIds)
                .GreaterThan(0).WithMessage("Debe seleccionar compañías válidas.");
        }
    }
}