using Client.Services.Interfaces;
using LanguageExt.Common;
using Shared.Common;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.PaymentDTOs;

namespace Client.Services.Implementation
{
    public class BCartService : BICartService
    {
        private readonly IHttpClientHelper _httpClientHelper;

        public BCartService(IHttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }

        public async Task<Result<BaseResponse<CartDto>>> GetCartByUserIdAsync(string id, bool useCache, CancellationToken cancellationToken = default)
        {
            var response = await _httpClientHelper.GetBaseAsync<BaseResponse<CartDto>>($"api/Cart/GetByUser?userId={id}&useCache={useCache}");
            return response;
        }

        public async Task<Result<BaseResponse<AddItemResponse>>> AddToCartAsync(AddItemRequest addItemRequest)
        {
            var response = await _httpClientHelper.PostBaseAsync<AddItemRequest, BaseResponse<AddItemResponse>>("api/Cart/AddItem", addItemRequest);
            return response;
        }
        public async Task<Result<BaseResponse<RemoveItemResponse>>> RemoveItemAsync(RemoveItemRequest removeItemRequest, CancellationToken cancellationToken = default)
        {
            var response = await _httpClientHelper.PostBaseAsync<RemoveItemRequest, BaseResponse<RemoveItemResponse>>("api/Cart/RemoveItem", removeItemRequest);
            return response;
        }

        public async Task<Result<BaseResponse<bool>>> CheckoutAsync(CheckoutRequestDto checkoutRequest, CancellationToken cancellationToken = default)
        {
            var response = await _httpClientHelper.PostBaseAsync<CheckoutRequestDto, BaseResponse<bool>>("api/Cart/Checkout", checkoutRequest);
            return response;
        }

        public async Task<Result<BaseResponse<PaymentResponseDto>>> PayOrderAsync(PaymentRequestDto paymentRequestDto, CancellationToken cancellationToken = default)
        {
            var response = await _httpClientHelper.PostBaseAsync<PaymentRequestDto, BaseResponse<PaymentResponseDto>>("api/Payments/Request", paymentRequestDto);
            return response;
        }
    }
}
