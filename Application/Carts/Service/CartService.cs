using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Cart;
using Shared.DTOs.CartDTOs;
using Shared.Exceptions;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class CartService : BaseService<Cart>, ICartService
    {
        public CartService(IUnitOfWork unitOfWork, ILogger<Cart> logger, IMapper mapper, ICurrentUser currentUser) : base(unitOfWork, logger, mapper)
        {
            _currentUser = currentUser;
        }

        //private readonly IPaymentService _paymentService;
        private readonly ICurrentUser _currentUser;

        public async Task<BaseResponse<CartDto>> GetForCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            var cartItemRepo = _unitOfWork.Repository<CartItem>();
            var cartItems = await cartItemRepo
                .Query()
                .Where(c => c.Cart.UserId == _currentUser.Id && !c.IsCheckedOut)
                .GroupBy(ci => new
                {
                    ci.Ticket.MatchTeamId,
                    ci.Ticket.Price,
                    ci.Ticket.Notes,
                    ci.Ticket.BlockId,
                    ci.Ticket.SeatId,
                    ci.Ticket.DisplayForSale,
                    ci.Ticket.Location,
                    ci.Ticket.Class,
                    ci.Ticket.SeatNumber,
                    ci.Ticket.ExternalGate,
                    ci.Ticket.InternalGate
                })
                .Select(g => new CartItemDto
                {
                    Id = g.First().Id,
                    TicketTeam = g.First().Ticket.MatchTeam.Team.Name,
                    HomeTeam = g.First().Ticket.MatchTeam.Match.MatchTeams!.First(mt => mt.IsHomeTeam).Team.Name,
                    AwayTeam = g.First().Ticket.MatchTeam.Match.MatchTeams!.First(mt => !mt.IsHomeTeam).Team.Name,
                    Class = g.First().Ticket.Class,
                    Location = g.First().Ticket.Location,
                    UnitPrice = g.First().Ticket.Price,
                    Quantity = g.Count(),
                    TotalPrice = g.First().Ticket.Price * g.Count()
                }).ToListAsync();

            return new CartDto
            {
                CartItems = cartItems,
                Total = cartItems.Sum(ci => ci.TotalPrice)
            };
        }

        public async Task<BaseResponse<Empty>> AddCartItemForCurrentUserAsync(AddCartItemDto dto, CancellationToken cancellationToken = default)
        {
            var matchRepo = _unitOfWork.Repository<Match>();
            var cartItemRepo = _unitOfWork.Repository<CartItem>();
            var cartRepo = _unitOfWork.Repository<Cart>();

            // load match of the ticket
            var match = await matchRepo
                .FirstOrDefaultAsync(m => m.MatchTeams!.Any(mt => mt.Tickets.Any(t => t.Id == dto.TicketId)), cancellationToken: cancellationToken);
            
            // check if user exceeds the max tickets count
            if (await cartItemRepo.CountAsync(ci => 
                    ci.Cart.UserId == _currentUser.Id && 
                    ci.Ticket.MatchTeam.Match.Id == match!.Id) >= match!.MaxPerUser)
                return new BadRequestException(
                [
                    new ErrorInfo 
                    { 
                        Title = Resource.PurchaseOrder_Invalid,
                        Message = Resource.PurchaseOrder_Invalid_Message 
                    }
                ]);

            // get user cart
            var cart = await cartRepo.FirstOrDefaultAsync(c => c.UserId == _currentUser.Id,
                [
                    nameof(Cart.CartItems)
                ], 
                cancellationToken);

            if (cart == null)
                cart = new Cart
                {
                    UserId = _currentUser.Id!.Value,
                    CartItems = []
                };

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                TicketId = dto.TicketId
            };

            // add new item to the user cart
      
            cart.CartItems ??= [];
            cart.CartItems.Add(cartItem);

            // update user cart
            cartRepo.Update(cart);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Empty.Default;
        } 
        
        public async Task<BaseResponse<Empty>> DeleteCartItemForCurrentUserAsync(DeleteCartItemDto dto, CancellationToken cancellationToken = default)
        {
            var cartRepo = _unitOfWork.Repository<Cart>();

            // get user cart
            var cart = await cartRepo.FirstOrDefaultAsync(c => c.UserId == _currentUser.Id,
                [
                    nameof(Cart.CartItems)
                ], 
                cancellationToken);

            if (cart == null)
                cart = new Cart
                {
                    UserId = _currentUser.Id!.Value,
                    CartItems = []
                };

            // delete cart item for current user
            if (cart.CartItems != null)
            {
                var cartItem = cart.CartItems
                    .FirstOrDefault(ci => ci.Id == dto.CartItemId);

                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);

                    // update user cart
                    cartRepo.Update(cart);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            return Empty.Default;
        }

        public async Task<BaseResponse<Empty>> CheckoutCartItemsForCurrentUserAsync(CheckoutCartDto dto, CancellationToken cancellationToken = default)
        {
            var cartRepo = _unitOfWork.Repository<Cart>();
            var orderRepo = _unitOfWork.Repository<Order>();

            var cart = await cartRepo.FirstOrDefaultAsync(c => c.UserId == _currentUser.Id,
                [
                    nameof(Cart.CartItems),
                    string.Join(".",
                        [
                            nameof(Cart.CartItems),
                            nameof(CartItem.Ticket)
                        ])
                ]);

            // check if cart is empty
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any(ci => !ci.IsCheckedOut))
                return new BadRequestException(
                [
                    new ErrorInfo 
                    {
                        Title = Resource.Checkout_Error,
                        Message = Resource.Checkout_Cart_Empty
                    }
                ]);

            var checkoutCartItems = cart.CartItems.Where(ci => !ci.IsCheckedOut);

            // prepare orders
            var order = new Order
            {
                UserId = _currentUser.Id!.Value,
                PaymentUrl = dto.PaymentUrl,
                PaymentOrderRef = dto.PaymentRef,
                PaymentStatus = (int)PaymentConfiguration.PaymentStatusEnum.Paid,
                TotalAmount = checkoutCartItems.Aggregate(0m, (acc, ci) => acc + ci.Ticket.Price)
            };

            // prepare order items
            order.OrderItems = checkoutCartItems.Select(ci => new OrderItem
            {
                TicketId = ci.TicketId,
                Price = ci.Ticket.Price
            })
             .ToArray();

            // update tickets
            foreach(var cartItem in checkoutCartItems)
            {
                cartItem.IsCheckedOut = true;
                cartItem.Ticket.TicketStatus = TicketStatus.Sold;
            }

            orderRepo.Create(order);
            cartRepo.Update(cart);

            await _unitOfWork.SaveChangesAsync();

            return Empty.Default;
        }
    }
}
