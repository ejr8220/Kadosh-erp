using Application.Dtos.Request.General;
using FluentValidation;

namespace Application.Validators.General
{
    public class BranchDtoValidator : AbstractValidator<BranchRequestDto>
    {
        public BranchDtoValidator()
        {
            RuleFor(b => b.Code)
                .NotEmpty().WithMessage("El código de la sucursal es obligatorio.")
                .MaximumLength(20).WithMessage("El código no puede superar los 20 caracteres.");

            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("El nombre de la sucursal es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(b => b.Address)
                .MaximumLength(200).WithMessage("La dirección no puede superar los 200 caracteres.");

            RuleFor(b => b.Phone)
                .MaximumLength(20).WithMessage("El teléfono no puede superar los 20 caracteres.");

            RuleFor(b => b.Email)
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
                .MaximumLength(100).WithMessage("El correo electrónico no puede superar los 100 caracteres.");

            RuleFor(b => b.CompanyId)
                .GreaterThan(0).WithMessage("Debe seleccionar una compañía válida.");
        }
    }
}