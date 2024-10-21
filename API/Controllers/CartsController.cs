using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Cart;
using Shared.DTOs.CartDTOs;

namespace API.Controllers
{
    [Authorize]
    public class CartsController : BaseController
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("get-for-current-user")]
        public async Task<IActionResult> GetForCurrentUserAsync(CancellationToken cancellationToken = default)
            => Result(await _cartService.GetForCurrentUserAsync(cancellationToken));

        [HttpPost("add-cart-item-for-current-user")]
        public async Task<IActionResult> AddCartItemForCurrentUserAsync(AddCartItemDto dto, CancellationToken cancellationToken = default)
            => Result(await _cartService.AddCartItemForCurrentUserAsync(dto, cancellationToken));
        
        [HttpDelete("delete-cart-item-for-current-user")]
        public async Task<IActionResult> DeleteCartItemForCurrentUserAsync(DeleteCartItemDto dto, CancellationToken cancellationToken = default)
            => Result(await _cartService.DeleteCartItemForCurrentUserAsync(dto, cancellationToken));

        [HttpPost("Checkout-cart-items-for-current-user")]
        public async Task<IActionResult> Checkout(CancellationToken cancellationToken = default)
            => Result(await _cartService.CheckoutCartItemsForCurrentUserAsync(cancellationToken));
    }
}
