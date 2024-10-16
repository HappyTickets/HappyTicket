using FluentValidation;
using Shared.DTOs.MatchDtos;

namespace Application.Validation
{
    public partial class TicketDtoValidator
    {
        public class MatchValidator : AbstractValidator<MatchDto>
        {
            public MatchValidator()
            {
                RuleFor(x => x.StadiumId).NotNull().WithMessage("Stadium is required.");
                RuleFor(x => x.EventDate).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Match date must be in the future.");
            }
        }
    }
}
