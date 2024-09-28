using FluentValidation;
using Shared.DTOs.CartDTOs;

namespace Application.Validation;

public class CartValidator : AbstractValidator<CartDto>
{
    public CartValidator()
    {
    }
}
