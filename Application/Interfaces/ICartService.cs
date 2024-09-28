using LanguageExt.Common;
using LanguageExt;
using Shared.DTOs.CartDTOs;
using Domain.Entities.CartEntity;
using Shared.DTOs.PaymentDTOs;

namespace Application.Interfaces
{
    public interface ICartService : IBaseService<Cart, CartDto>
    {
        Task<Result<CartDto>> GetByUserAsync(string userId, bool useCache = true, CancellationToken cancellationToken = default);
        Task<Result<AddItemResponse>> AddItemAsync(AddItemRequest addItemRequest, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveItemAsync(RemoveItemRequest removeItemRequest, CancellationToken cancellationToken = default);
        Task<Result<bool>> CheckoutAsync(CheckoutRequestDto checkoutRequestDto, CancellationToken cancellationToken = default);    }
}
