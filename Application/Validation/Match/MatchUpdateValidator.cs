using FluentValidation;
using Shared.DTOs.MatchDtos;
using Shared.ResourceFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation.Match
{
    public class MatchUpdateValidator : AbstractValidator<UpdateMatchDto>
    {
        public MatchUpdateValidator()
        {
            RuleFor(x => x.TeamAId).NotNull().WithMessage(Resource.HomeTeamIsRequired);
            RuleFor(x => x.TeamBId).NotNull().WithMessage(Resource.AwayTeamIsRequired);
            RuleFor(x => x.StadiumId).NotNull().WithMessage(Resource.StadiumIsRequired);
            RuleFor(x => x.ChampionId).NotNull().WithMessage(Resource.ChampionshipIsRequired);
            RuleFor(x => x.EventDate).GreaterThanOrEqualTo(DateTime.Now).WithMessage(Resource.DateMustBeInFuture);
        }
    }
}
