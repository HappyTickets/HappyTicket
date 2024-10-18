using FluentValidation;
using Shared.DTOs.Sponsors;

namespace Application.Sponsors.Validators
{
    public class CreateOrUpdateSponsorValidator : AbstractValidator<CreateOrUpdateSponsorDto>
    {
        public CreateOrUpdateSponsorValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty();
        }
    }
}
