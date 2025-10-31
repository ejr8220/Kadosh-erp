using Application.Dtos.Request.Security;
using FluentValidation;

namespace Application.Validators.Security
{
    public class UserRoleDtoValidator : AbstractValidator<UserRoleRequestDto>
    {
        public UserRoleDtoValidator()
        {
            RuleFor(ur => ur.UserId)
                .GreaterThan(0).WithMessage("Debe seleccionar un usuario válido.");

            RuleFor(ur => ur.RoleId)
                .GreaterThan(0).WithMessage("Debe seleccionar un rol válido.");
        }
    }
}