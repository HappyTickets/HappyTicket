using Application.Common.Interfaces.Services;
using Domain.Entities;
using LanguageExt.Common;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Cart;
using Shared.DTOs.CartDTOs;

namespace Application.Interfaces
{
    public interface ICartService : IBaseService<Cart>
    {
        Task<BaseResponse<Empty>> AddCartItemForCurrentUserAsync(AddCartItemDto dto);
        Task<BaseResponse<string>> CheckoutCartItemsForCurrentUserAsync();
        Task<BaseResponse<Empty>> DeleteCartItemForCurrentUserAsync(DeleteCartItemDto dto);
        Task<BaseResponse<CartDto>> GetForCurrentUserAsync();
    }
}
