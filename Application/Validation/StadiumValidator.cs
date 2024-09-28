using FluentValidation;
using Shared.DTOs;

namespace Application.Validation
{
    public partial class TicketDtoValidator
    {
        public class StadiumValidator : AbstractValidator<StadiumDto>
        {
            public StadiumValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Stadium name is required.");
                RuleFor(x => x.Photo).NotEmpty().WithMessage("Photo is required.");
                //RuleForEach(x => x.Blocks).SetValidator(new BlockValidator());
            }
        }
    }
}
