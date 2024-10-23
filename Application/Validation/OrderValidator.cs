using FluentValidation;
using Shared.DTOs.OrderDtos.Response;

namespace Application.Validation
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {

        }
    }
}
