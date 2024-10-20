using FluentValidation;
using Shared.DTOs.MatchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.Match
{
    public class MatchValidator : AbstractValidator<MatchDto>
    {
        public MatchValidator()
        {
            RuleFor(x => x.TeamAId).NotNull().WithMessage("Home team is required.");
            RuleFor(x => x.TeamBId).NotNull().WithMessage("Away team is required.");
            RuleFor(x => x.StadiumId).NotNull().WithMessage("Stadium is required.");
            RuleFor(x => x.EventDate).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Match date must be in the future.");
        }
    }
}
