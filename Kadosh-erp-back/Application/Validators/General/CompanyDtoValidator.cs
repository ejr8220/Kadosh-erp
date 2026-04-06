using Application.Dtos.Request.General;
using FluentValidation;
using System;

namespace Application.Validators.General
{
    public class CompanyDtoValidator : AbstractValidator<CompanyRequestDto>
    {
        public CompanyDtoValidator()
        {
            RuleFor(c => c)
                .Must(HasPersonInformation)
                .WithMessage("Debe seleccionar una persona válida o enviar datos de identificación y ubicación para crearla.");

            RuleFor(c => ResolveLegalIdentification(c))
                .NotEmpty().WithMessage("La cédula del representante legal es obligatoria.")
                .MaximumLength(20).WithMessage("La cédula no puede superar los 20 caracteres.");

            RuleFor(c => ResolveLegalRepresentativeName(c))
                .NotEmpty().WithMessage("El nombre del representante legal es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(c => c.AccountantIdentification)
                .NotEmpty().WithMessage("La cédula del contador es obligatoria.")
                .MaximumLength(20).WithMessage("La cédula no puede superar los 20 caracteres.");

            RuleFor(c => ResolveAccountantName(c))
                .NotEmpty().WithMessage("El nombre del contador es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(c => c.LogoUrl)
                .MaximumLength(250).WithMessage("La URL del logo no puede superar los 250 caracteres.");

            RuleForEach(c => c.Branches)
                .ChildRules(branch =>
                {
                    branch.RuleFor(b => b.Code)
                        .NotEmpty().WithMessage("El código de la sucursal es obligatorio.")
                        .MaximumLength(20).WithMessage("El código de la sucursal no puede superar los 20 caracteres.");

                    branch.RuleFor(b => b.Name)
                        .NotEmpty().WithMessage("El nombre de la sucursal es obligatorio.")
                        .MaximumLength(100).WithMessage("El nombre de la sucursal no puede superar los 100 caracteres.");

                    branch.RuleFor(b => b.Email)
                        .EmailAddress().When(b => !string.IsNullOrWhiteSpace(b.Email))
                        .WithMessage("El correo electrónico de sucursal no tiene un formato válido.");
                });
        }

        private static bool HasPersonInformation(CompanyRequestDto dto)
        {
            if (dto.PersonId > 0)
                return true;

            return dto.IdentificationTypeId.HasValue
                && !string.IsNullOrWhiteSpace(dto.Identificacion)
                && dto.CountryId.HasValue
                && dto.ProvinceId.HasValue
                && dto.CityId.HasValue;
        }

        private static string ResolveLegalIdentification(CompanyRequestDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.LegalpresentativeIdentification))
                return dto.LegalpresentativeIdentification;

            if (!string.IsNullOrWhiteSpace(dto.LegalRepresentativeIdentification))
                return dto.LegalRepresentativeIdentification;

            return dto.Identificacion;
        }

        private static string ResolveLegalRepresentativeName(CompanyRequestDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.LegalRepresentative))
                return dto.LegalRepresentative;

            if (!string.IsNullOrWhiteSpace(dto.LegalRepresentativeName))
                return dto.LegalRepresentativeName;

            if (!string.IsNullOrWhiteSpace(dto.RazonSocial))
                return dto.RazonSocial;

            return dto.NombreComercial;
        }

        private static string ResolveAccountantName(CompanyRequestDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Accountant))
                return dto.Accountant;

            return dto.AccountantName;
        }
    }
}