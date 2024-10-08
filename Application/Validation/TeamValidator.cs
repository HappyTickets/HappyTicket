using FluentValidation;
using Shared.DTOs.Team;

namespace Application.Validation
{
    public partial class TicketDtoValidator
    {
        public class TeamValidator : AbstractValidator<TeamDto>
        {
            public TeamValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Team name is required.");
                RuleFor(x => x.Logo).NotEmpty().WithMessage("Logo is required.");
            }
        }
    }
}
