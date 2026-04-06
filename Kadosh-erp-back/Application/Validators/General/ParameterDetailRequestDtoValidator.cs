using Application.Dtos.Request.General;
using FluentValidation;

namespace Application.Validators.General
{
    public class ParameterDetailRequestDtoValidator : AbstractValidator<ParameterDetailRequestDto>
    {
        public ParameterDetailRequestDtoValidator()
        {
            RuleFor(x => x.ParameterHeaderId)
                .GreaterThan(0);

            RuleFor(x => x.Value)
                .NotEmpty().MaximumLength(4000);
        }
    }
}
