using FluentValidation;
using Shared.DTOs.OrderDtos.Request;
using Shared.ResourceFiles;

namespace Application.Orders.Validators
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(dto => dto.OrderId)
                .NotEmpty()
                .WithMessage(Resource.RequiredField);

            RuleFor(dto => dto.TicketId)
                .NotEmpty()
                .WithMessage(Resource.RequiredField);

            RuleFor(dto => dto.Price)
                .NotEmpty()
                .WithMessage(Resource.RequiredField)
                .PrecisionScale(18, 2, true)
                .WithMessage(Resource.PricePrecision);
        }
    }


}
