using Application.Dtos.Request.General;
using FluentValidation;
using System;


namespace Application.Validators.General;
public class ZoneDtoValidator : AbstractValidator<ZoneRequestDto>
{
    public ZoneDtoValidator()
    {
        RuleFor(z => z.Name)
            .NotEmpty().WithMessage("El nombre de la zona es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(z => z.ParishId)
            .GreaterThan(0).WithMessage("Debe seleccionar una parroquia válida.");
    }
}

