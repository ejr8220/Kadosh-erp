using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General;
public class PersonDtoValidator : AbstractValidator<PersonRequestDto>
{
    public PersonDtoValidator()
    {
        RuleFor(p => p.IdentificationType)
            .IsInEnum().WithMessage("El tipo de identificación no es válido.");

        RuleFor(p => p.IdentificationNumber)
            .NotEmpty().WithMessage("El número de identificación es obligatorio.")
            .MaximumLength(30).WithMessage("El número de identificación no puede superar los 30 caracteres.");

        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

        RuleFor(p => p.BirthDate)
            .LessThan(DateTime.Today).When(p => p.BirthDate.HasValue)
            .WithMessage("La fecha de nacimiento debe ser anterior a la fecha actual.");

        RuleFor(p => p.Gender)
            .MaximumLength(10).WithMessage("El género no puede superar los 10 caracteres.");

        RuleFor(p => p.CountryId)
            .GreaterThan(0).WithMessage("Debe seleccionar un país válido.");

        RuleFor(p => p.ProvinceId)
            .GreaterThan(0).WithMessage("Debe seleccionar una provincia válida.");

        RuleFor(p => p.CityId)
            .GreaterThan(0).WithMessage("Debe seleccionar una ciudad válida.");

        RuleFor(p => p.ParishId)
            .GreaterThan(0).WithMessage("Debe seleccionar una parroquia válida.");
    }
}