using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General;
public class MaritalStatusDtoValidator : AbstractValidator<MaritalStatusRequestDto>
{
    public MaritalStatusDtoValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("El nombre del estado civil es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");
    }
}