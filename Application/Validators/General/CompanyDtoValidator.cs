using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General
{
    public class CompanyDtoValidator : AbstractValidator<CompanyRequestDto>
    {
        public CompanyDtoValidator()
        {
            RuleFor(c => c.PersonId)
                .GreaterThan(0).WithMessage("Debe seleccionar una persona válida.");

            RuleFor(c => c.LegalpresentativeIdentification)
                .NotEmpty().WithMessage("La cédula del representante legal es obligatoria.")
                .MaximumLength(20).WithMessage("La cédula no puede superar los 20 caracteres.");

            RuleFor(c => c.LegalRepresentative)
                .NotEmpty().WithMessage("El nombre del representante legal es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(c => c.AccountantIdentification)
                .NotEmpty().WithMessage("La cédula del contador es obligatoria.")
                .MaximumLength(20).WithMessage("La cédula no puede superar los 20 caracteres.");

            RuleFor(c => c.Accountant)
                .NotEmpty().WithMessage("El nombre del contador es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(c => c.LogoUrl)
                .MaximumLength(250).WithMessage("La URL del logo no puede superar los 250 caracteres.");
        }
    }
}