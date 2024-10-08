using FluentValidation;
using Shared.DTOs.Team;

namespace Application.Validation
{
    public class CreateOrUpdateTeamValidator: AbstractValidator<CreateOrUpdateTeamDto>
    {
        public CreateOrUpdateTeamValidator()
        {
            RuleFor(t => t.Name).NotEmpty();
            RuleFor(t => t.Logo).NotEmpty();
        }
    }
}
