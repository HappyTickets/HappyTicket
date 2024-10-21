using FluentValidation;
using Shared.DTOs.OrderDtos.Request;
using Shared.ResourceFiles;

namespace Application.Orders.Validators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(dto => dto.UserId)
                .NotEmpty()
                .WithMessage(Resource.RequiredField);

            RuleFor(dto => dto.TotalAmount)
                            .GreaterThanOrEqualTo(0);

            RuleForEach(dto => dto.OrderItems)
                .SetValidator(new CreateOrderItemDtoValidator());
        }
    }

}
