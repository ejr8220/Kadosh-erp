using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class RolePermissionDtoValidator : AbstractValidator<RolePermissionRequestDto>
    {
        public RolePermissionDtoValidator()
        {
            RuleFor(rp => rp.RoleId)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido.");

            RuleFor(rp => rp.PermissionId)
                .GreaterThan(0).WithMessage("Debe seleccionar un permiso válido.");
        }
    }
}