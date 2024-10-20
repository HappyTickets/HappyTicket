//using FluentValidation;
//using Shared.DTOs.MatchDtos;

//namespace Application.Validation
//{
//    public partial class TicketDtoValidator
//    {
//        public class MatchCreateValidator : AbstractValidator<CreateMatchDto>
//        {
//            public MatchCreateValidator()
//            {
//                RuleFor(x => x.TeamAId).NotNull().WithMessage("Home team is required.");
//                RuleFor(x => x.TeamBId).NotNull().WithMessage("Away team is required.");
//                RuleFor(x => x.StadiumId).NotNull().WithMessage("Stadium is required.");
//                RuleFor(x => x.ChampionId).NotNull().WithMessage("Champion is required.");
//                RuleFor(x => x.EventDate).GreaterThanOrEqualTo(DateTime.Now).WithMessage("Match date must be in the future.");
//            }
//        }
//    }
//}
