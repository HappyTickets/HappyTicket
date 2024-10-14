using Application.Interfaces;
using Application.Interfaces.ITicketServices;
using Application.Interfaces.PaymentServices;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.CartEntity;
using Domain.Enums;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.Common.General;
using Shared.DTOs.CartDTOs;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class CartService : BaseService<Cart, CartDto>, ICartService
    {
        private readonly IPaymentService _paymentService;
        private readonly ITicketService _ticketService;



        public CartService(IUnitOfWork unitOfWork, ILogger<Cart> logger, IMemoryCache cache, IMapper mapper, IValidator<CartDto> validator, IStringLocalizer<Resource> localizer, IPaymentService paymentService, ITicketService ticketvalidator) : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
            _paymentService = paymentService;
            _ticketService = ticketvalidator;
        }

        public async Task<Result<CartDto>> GetByUserAsync(string userId, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.FirstOrDefaultAsync(c => c.UserId == userId, useCache, cancellationToken: cancellationToken, includeDTOProperties: c => c.Include(x => x.CartItems)!.ThenInclude(x => x.Ticket).ThenInclude(x => x!.Team)
                                                                                                                                                           .Include(x => x.CartItems)!.ThenInclude(x => x.Ticket).ThenInclude(x => x!.Match).ThenInclude(x => x!.TeamA)
                                                                                                                                                           .Include(x => x.CartItems)!.ThenInclude(x => x.Ticket).ThenInclude(x => x!.Match).ThenInclude(x => x!.TeamB)!);
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }
        public async Task<Result<AddItemResponse>> AddItemAsync(AddItemRequest addItemRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                // Retrieve the ticket
                var getTicketResult = await _unitOfWork.Repository<Ticket>().FirstOrDefaultAsync(
                    x => x.Id == addItemRequest.Item.TicketId,
                    cancellationToken,
                    x => x.Include(t => t.Match!)
                );

                return await getTicketResult.Match<Task<Result<AddItemResponse>>>(
                    async getTicketSucc =>
                    {
                        var getCartResult = await _unitOfWork.Repository<Cart>().FirstOrDefaultAsync(
                            c => c.UserId == addItemRequest.UserId,
                            cancellationToken,
                            x => x.Include(c => c.CartItems!).ThenInclude(ci => ci.Ticket!)
                        );

                        return await getCartResult.Match<Task<Result<AddItemResponse>>>(
                            async getCartSucc =>
                            {
                                // Check if the user has reached the maximum allowed tickets for this match
                                if (getCartSucc.CartItems?.Count(x => x.Ticket?.MatchId == getTicketSucc.MatchId) >= getTicketSucc.Match?.MaxPerUser) return new(new BadRequestException([new() { Title = Resource.PurchaseOrder_Invalid, Message = Resource.PurchaseOrder_Invalid_Message }]));

                                // Map the single cart item
                                var cartItem = _mapper.Map<CartItem>(addItemRequest.Item);
                                cartItem.Cart = getCartSucc;
                                cartItem.CartId = getCartSucc.Id;
                                getTicketSucc.TicketStatus = TicketStatus.InCart;

                                // Save the cart item
                                return await _unitOfWork.Repository<CartItem>().CreateAsync(cartItem).MapAsync(
                                    async updateSucc =>
                                    {
                                        var result = await _unitOfWork.SaveChangesAsync(cancellationToken).Map(succ => new AddItemResponse());
                                        return result;
                                    });
                            },
                            async getCartFail => await getCartFail.ToResultAsync<AddItemResponse>(cancellationToken)
                        );
                    },
                    async getTicketFail => await getTicketFail.ToResultAsync<AddItemResponse>(cancellationToken)
                );
            }
            catch (Exception ex)
            {
                return new Result<AddItemResponse>(ex);
            }
        }


        public async Task<Result<Unit>> RemoveItemAsync(RemoveItemRequest removeItemRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                // Fetch the CartItem by its ID
                var cartItemResult = await _unitOfWork.Repository<CartItem>().GetByIdAsync(removeItemRequest.ItemId, cancellationToken);

                return await cartItemResult.Match(
                async cartItemSucc =>
                {
                    _unitOfWork.Repository<CartItem>().ClearTrack();

                    return await (await _unitOfWork.Repository<Ticket>().GetByIdAsync(cartItemSucc.TicketId)).Match(
                        async succ =>
                        {
                            succ.TicketStatus = TicketStatus.Active;

                            _unitOfWork.Repository<Ticket>().ModifyUpdateState(succ);

                            return await _unitOfWork.Repository<CartItem>().HardDelete(cartItemSucc).Match(
                                async updateSucc =>
                                {
                                    return await _unitOfWork.SaveChangesAsync(cancellationToken).Map(succ => new Result<Unit>(new Unit()));
                                },
                                async Fail =>
                                {
                                    await Task.Delay(0);
                                    return new Result<Unit>(Fail);
                                }
                            );
                        },
                        async Fail =>
                        {
                            await Task.Delay(0);
                            return new Result<Unit>(Fail);
                        }
                    );
                },
                async Fail =>
                {
                    await Task.Delay(0);
                    return new Result<Unit>(Fail);
                });
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }


        public async Task<Result<bool>> CheckoutAsync(CheckoutRequestDto checkoutRequestDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var cartResult = await _unitOfWork.Repository<Cart>().FirstOrDefaultAsync(
                    c => c.UserId == checkoutRequestDto.UserId,
                    cancellationToken,
                    c => c.Include(x => x.User)!,
                    c => c.Include(x => x.CartItems)!.ThenInclude(x => x.Ticket)!.ThenInclude(x => x.Team)!,
                    c => c.Include(x => x.CartItems)!.ThenInclude(x => x.Ticket)!.ThenInclude(x => x.Match)!.ThenInclude(x => x.Stadium)!,
                    c => c.Include(x => x.CartItems)!.ThenInclude(x => x.Ticket)!.ThenInclude(x => x.Match)!.ThenInclude(x => x.TeamA)!,
                    c => c.Include(x => x.CartItems)!.ThenInclude(x => x.Ticket)!.ThenInclude(x => x.Match)!.ThenInclude(x => x.TeamB)!
                );

                return await cartResult.Match(
                    async succ =>
                    {
                        // Initialize CartItems if null and filter out checked-out items
                        succ.CartItems ??= [];
                        var notCheckedoutItems = succ.CartItems.Where(x => !x.IsCheckedOut).ToList();

                        if (notCheckedoutItems.Any())
                        {
                            // Create a new order
                            OrderO newOrder = new()
                            {
                                Id = Guid.NewGuid(),
                                UserId = checkoutRequestDto.UserId,
                                TotalAmount = notCheckedoutItems.Select(x => x.Ticket!.Price).Aggregate(0.0m, (seed, price) => seed + price),
                                PaymentUrl = checkoutRequestDto.PaymentUrl,
                                PaymentOrderRef = checkoutRequestDto.PaymentRef,
                                PaymentStatus = (int)PaymentConfiguration.PaymentStatusEnum.Paid,
                                CartItems = notCheckedoutItems,
                            };

                            // Update CartItems and Ticket status
                            foreach (var item in notCheckedoutItems)
                            {
                                item.IsCheckedOut = true;
                                item.OrderId = newOrder.Id;

                                if (item.Ticket != null)
                                {
                                    // Set TicketStatus to Sold
                                    item.Ticket.TicketStatus = TicketStatus.Sold;
                                    // Mark the ticket as modified
                                    _unitOfWork.Repository<Ticket>().ModifyUpdateState(item.Ticket);
                                }
                            }

                            // Update the Cart and create the Order
                            var result = await _unitOfWork.Repository<Cart>().Update(succ).Match(
                                async updateSucc => await (await _unitOfWork.Repository<OrderO>().CreateAsync(newOrder)).Match(
                                        async createSucc => (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(succ => new Unit()),
                                        async createFail => await createFail.ToResultAsync<Unit>()),
                                async updateFail => await updateFail.ToResultAsync<Unit>()
                            );

                            // Send confirmation email for tickets
                            //await _paymentService.SendTicketEmailAsync(succ.User.Email, notCheckedoutItems, cancellationToken);

                            return new Result<bool>(true);
                        }
                        else
                        {
                            return new Result<bool>(false);
                        }
                    },
                    async fail => new Result<bool>(false)
                );
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }
    }
}
