using Application.Dtos.Request.General;
using FluentValidation;

namespace Application.Validators.General
{
    public class ParameterHeaderRequestDtoValidator : AbstractValidator<ParameterHeaderRequestDto>
    {
        public ParameterHeaderRequestDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().MaximumLength(80);

            RuleFor(x => x.Name)
                .NotEmpty().MaximumLength(120);
        }
    }
}
