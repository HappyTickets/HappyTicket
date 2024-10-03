using FluentValidation;
using Shared.DTOs;

namespace Application.Validation
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {

        }
    }
}
