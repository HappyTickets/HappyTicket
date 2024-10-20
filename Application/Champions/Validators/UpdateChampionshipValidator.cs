using FluentValidation;
using Shared.DTOs.ChampionDtos;

namespace Application.Champions.Validators
{
    public class UpdateChampionshipValidator : AbstractValidator<UpdateChampionshipDto>
    {
        public UpdateChampionshipValidator()
        {
            RuleFor(result => result.Id).NotEmpty().WithMessage("Id Is Required");
            RuleFor(result => result.Name).NotEmpty().WithMessage("Name Is Required");
            RuleFor(result => result.Logo).NotEmpty().WithMessage("Logo Is Required");
        }
    }
}
