using FluentValidation;
using Shared.DTOs.ChampionDtos;

namespace Application.Champions.Validators
{
    public class UpdateChampionshipValidator : AbstractValidator<UpdateChampionshipDto>
    {
        public UpdateChampionshipValidator()
        {
            RuleFor(result => result.Id).NotEmpty();
            RuleFor(result => result.Name).NotEmpty();
            RuleFor(result => result.Logo).NotEmpty();
        }
    }
}
