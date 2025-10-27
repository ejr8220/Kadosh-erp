using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General;
public class ParishDtoValidator : AbstractValidator<ParishRequestDto>
{
    public ParishDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre de la parroquia es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(p => p.CityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una ciudad válida.");
    }
}