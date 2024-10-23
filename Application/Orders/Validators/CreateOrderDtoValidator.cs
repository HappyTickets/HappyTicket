using FluentValidation;
using Shared.DTOs.OrderDtos.Request;

namespace Application.Orders.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {


            RuleFor(dto => dto.TotalAmount)
                            .GreaterThanOrEqualTo(0);

            RuleForEach(dto => dto.OrderItems)
                .SetValidator(new CreateOrderItemDtoValidator());
        }
    }

}
