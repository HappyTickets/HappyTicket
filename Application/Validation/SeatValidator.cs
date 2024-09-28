using FluentValidation;
using Shared.DTOs;

namespace Application.Validation
{
    public partial class TicketDtoValidator
    {
        public class SeatValidator : AbstractValidator<SeatDto>
        {
            public SeatValidator()
            {
                RuleFor(x => x.Number).NotEmpty().WithMessage("Seat number is required.");
            }
        }
    }

}