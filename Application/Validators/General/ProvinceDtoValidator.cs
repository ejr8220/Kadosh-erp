using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General;
public class ProvinceDtoValidator : AbstractValidator<ProvinceRequestDto>
{
    public ProvinceDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre de la provincia es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(p => p.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país válido.");
    }
}