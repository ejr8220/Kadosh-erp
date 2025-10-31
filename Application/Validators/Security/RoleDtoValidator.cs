using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class RoleDtoValidator : AbstractValidator<RoleRequestDto>
    {
        public RoleDtoValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleForEach(r => r.PermissionIds)
                .GreaterThan(0).WithMessage("Cada ID de permiso debe ser mayor a cero.");
        }
    }
}