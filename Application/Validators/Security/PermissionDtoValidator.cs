using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class PermissionDtoValidator : AbstractValidator<PermissionRequestDto>
    {
        public PermissionDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre del permiso es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(p => p.Description)
                .MaximumLength(250).WithMessage("La descripción no puede superar los 250 caracteres.");

            RuleForEach(p => p.RoleIds)
                .GreaterThan(0).WithMessage("Debe seleccionar roles válidos.");
        }
    }
}