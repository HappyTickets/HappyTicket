using FluentValidation;
using Shared.DTOs.Champion;

namespace Application.Validation
{
    public class CreateOrUpdateChampionValidator : AbstractValidator<CreateOrUpdateChampionDto>
    {
        public CreateOrUpdateChampionValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Logo).NotEmpty();
        }
    }
}
