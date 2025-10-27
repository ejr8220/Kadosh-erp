using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General;
public class CountryDtoValidator : AbstractValidator<CountryRequestDto>
{
    public CountryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("El nombre del país es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(c => c.IsoCode)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(3).WithMessage("El código ISO no puede superar los 3 caracteres.")
            .Matches("^[A-Za-z]{2,3}$").WithMessage("El código ISO debe contener solo letras y tener 2 o 3 caracteres.")
            .When(c => !string.IsNullOrWhiteSpace(c.IsoCode));
    }
}