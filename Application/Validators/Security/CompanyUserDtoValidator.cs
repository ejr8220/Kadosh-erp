using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class CompanyUserDtoValidator : AbstractValidator<CompanyUserRequestDto>
    {
        public CompanyUserDtoValidator()
        {
            RuleFor(cu => cu.CompanyId)
                .GreaterThan(0).WithMessage("Debe seleccionar una empresa válida.");

            RuleFor(cu => cu.UserId)
                .GreaterThan(0).WithMessage("Debe seleccionar un usuario válido.");
        }
    }
}