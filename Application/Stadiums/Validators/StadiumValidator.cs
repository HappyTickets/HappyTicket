using FluentValidation;
using Shared.DTOs.StadiumDTO;

namespace Application.Validation
{
    public partial class TicketDtoValidator
    {
        public class StadiumValidator : AbstractValidator<CreateStadiumDto>
        {
            public StadiumValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Stadium name is required.");
                RuleFor(x => x.Photo).NotEmpty().WithMessage("Photo is required.");
                RuleFor(x => x.Location).NotEmpty().WithMessage("Location is required.");
            }
        }
    }
}
