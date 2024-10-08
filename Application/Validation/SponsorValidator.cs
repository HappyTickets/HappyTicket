using FluentValidation;
using Shared.DTOs;

namespace Application.Validation
{
    public class SponsorValidator : AbstractValidator<SponsorDto>
    {
        public SponsorValidator()
        {
            RuleFor(s => s.Name).NotEmpty();
            RuleFor(s => s.Logo).NotEmpty();
        }
    }
}
