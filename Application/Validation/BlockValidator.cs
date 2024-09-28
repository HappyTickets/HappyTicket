using FluentValidation;
using Shared.DTOs;

namespace Application.Validation
{
    public partial class TicketDtoValidator
    {
        public class BlockValidator : AbstractValidator<BlockDto>
        {
            public BlockValidator()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Block name is required.");
                RuleForEach(x => x.Seats).SetValidator(new SeatValidator());
            }
        }
    }
}
