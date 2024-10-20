using FluentValidation;
using Shared.DTOs.Champion;

namespace Application.Champions.Validators
{
    public class CreateChampionValidator : AbstractValidator<CreateChampionshipDto>
    {
        public CreateChampionValidator()
        {
            RuleFor(result => result.Name).NotEmpty();
            RuleFor(result => result.Logo).NotEmpty();
        }
    }
}
