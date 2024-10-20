using FluentValidation;
using Shared.DTOs.Champion;

namespace Application.Champions.Validators
{
    public class CreateChampionValidator : AbstractValidator<CreateChampionshipDto>
    {
        public CreateChampionValidator()
        {
            RuleFor(result => result.Name).NotEmpty().WithMessage("Name Is Required");
            RuleFor(result => result.Logo).NotEmpty().WithMessage("Logo Is Required");
        }
    }
}
