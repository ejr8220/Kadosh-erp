using Application.Dtos.Request.Tax;
using FluentValidation;
using System;

namespace Application.Validators.Tax;
public class IdentificationTypeDtoValidator : AbstractValidator<IdentificationTypeRequestDto>
{
    public IdentificationTypeDtoValidator()
    {
        RuleFor(i => i.Name)
            .NotEmpty().WithMessage("El nombre del tipo de identificación es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(i => i.Code)
            .NotEmpty().WithMessage("El código es obligatorio.")
            .MaximumLength(10).WithMessage("El código no puede superar los 10 caracteres.")
            .Matches(@"^\S+$").WithMessage("El código no puede contener espacios.");

        RuleFor(i => i.Maxlength)
            .GreaterThan(0).WithMessage("La longitud máxima debe ser mayor que 0.");
    }
}