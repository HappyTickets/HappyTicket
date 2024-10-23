using FluentValidation;
using Shared.ResourceFiles;

namespace Application.Orders.Validators
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty()
                .WithMessage(Resource.RequiredField);

            RuleFor(dto => dto.TotalAmount)
                .GreaterThanOrEqualTo(0);

            RuleForEach(dto => dto.OrderItems)
                .SetValidator(new UpdateOrderItemDtoValidator());
        }
    }

}
