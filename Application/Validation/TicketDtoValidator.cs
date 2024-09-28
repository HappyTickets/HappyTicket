using FluentValidation;
using Shared.DTOs.TicketDTOs;

namespace Application.Validation
{
    public class TicketDTOValidator : AbstractValidator<TicketDto>
    {
        public TicketDTOValidator()
        {
            //RuleFor(dto => dto.SerialNumber)
            //    .NotEmpty().WithMessage("Ticket Serial Number is required.")
            //    .MaximumLength(100).WithMessage("Ticket Serial Number must not exceed 100 characters.");

            //RuleFor(dto => dto.Description)
            //    .NotEmpty().WithMessage("Ticket Description is required.")
            //    .MaximumLength(1000).WithMessage("Ticket Description must not exceed 1000 characters.");

            //RuleFor(dto => dto.SeatNumber)
            //    .NotEmpty().WithMessage("Ticket Seat Number is required.")
            //    .MaximumLength(100).WithMessage("Ticket Seat Number must not exceed 100 characters.");

            //RuleFor(dto => dto.QRCodeUrl)
            //    .NotEmpty().WithMessage("Ticket QRCode Url is required.")
            //    .MaximumLength(100).WithMessage("Ticket QRCode Url must not exceed 100 characters.");

            //RuleFor(dto => dto.BarCodeUrl)
            //    .NotEmpty().WithMessage("Ticket BarCode Url is required.")
            //    .MaximumLength(100).WithMessage("Ticket BarCode Url must not exceed 100 characters.");

            //RuleFor(dto => dto.TicketStatus)
            //    .NotEmpty().WithMessage("Ticket status is required.")
            //    .IsInEnum().WithMessage("Invalid ticket status.");

            //RuleFor(dto => dto.EventId).NotNull().NotEmpty().NotEqual(Guid.Empty).WithMessage("Event ID is required.");
        }
    }
}
