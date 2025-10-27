using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General;
public class CityDtoValidator : AbstractValidator<CityRequestDto>
{
    public CityDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("El nombre de la ciudad es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(c => c.ProvinceId)
            .GreaterThan(0).WithMessage("Debe seleccionar una provincia válida.");
    }
}