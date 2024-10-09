using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Shared.DTOs.CartDTOs;

namespace API.Controllers
{
    public class CartController : BaseController
    {
        private readonly string? _userId = null;
        private readonly ICartService _cartService;
        private readonly IStringLocalizerFactory _factory;

        public CartController(IHttpContextAccessor httpContextAccessor, ICartService cartService) : base(httpContextAccessor)
        {
            _userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            _cartService = cartService;
        }

        [HttpGet]
        [Route("GetByUser")]
        public async Task<ActionResult> GetByUser(string userId, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _cartService.GetByUserAsync(userId, useCache, cancellationToken: cancellationToken);
                return ReturnResult(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }

        [HttpPost]
        [Route("AddItem")]
        public async Task<ActionResult> AddItem(AddItemRequest addItemRequest, bool useCache = true, CancellationToken cancellationToken = default)

        {
            try
            {
                var result = await _cartService.AddItemAsync(addItemRequest, cancellationToken: cancellationToken);
                return ReturnResult(result);
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
        [HttpPost]
        [Route("RemoveItem")]
        public async Task<ActionResult> RemoveItem(RemoveItemRequest removeItemRequest, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return ReturnResult(await _cartService.RemoveItemAsync(removeItemRequest, cancellationToken: cancellationToken));
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
        [HttpPost]
        [Route("Checkout")]
        public async Task<ActionResult> Checkout(CheckoutRequestDto checkoutRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                return ReturnResult(await _cartService.CheckoutAsync(checkoutRequest, cancellationToken: cancellationToken));
            }
            catch (Exception ex)
            {
                return ReturnException(ex);
            }
        }
    }
}
