using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.PaymentDTOs;

namespace Client.Services.Interfaces
{
    public interface BICartService
    {
        Task<Result<BaseResponse<CartDto>>> GetCartByUserIdAsync(string id, bool useCache, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<AddItemResponse>>> AddToCartAsync(AddItemRequest addItemRequest);
        Task<Result<BaseResponse<RemoveItemResponse>>> RemoveItemAsync(RemoveItemRequest removeItemRequest, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<bool>>> CheckoutAsync(CheckoutRequestDto checkoutRequest, CancellationToken cancellationToken = default);
        Task<Result<BaseResponse<PaymentResponseDto>>> PayOrderAsync(PaymentRequestDto paymentRequestDto, CancellationToken cancellationToken = default);
    }
}