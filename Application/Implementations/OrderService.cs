using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.Identity.UserDTOs;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.TicketDTOs;
using Shared.Enums;
using Shared.ResourceFiles;
using System.Linq.Expressions;

namespace Application.Implementations
{
    public class OrderService : BaseService<Order, OrderDto>, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork, ILogger<Order> logger, IMemoryCache cache, IMapper mapper, IValidator<OrderDto> validator, IStringLocalizer<Resource> localizer)
            : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
        }

        public async Task<Result<IEnumerable<TicketDto>>> GetTicketsByUserIdAsync(string userId, bool useCache = false, CancellationToken cancellationToken = default)
        {
            try
            {
                // Include necessary properties: CartItems -> Tickets -> Match -> TeamA & TeamB
                Expression<Func<IQueryable<OrderDto>, IIncludableQueryable<OrderDto, object>>>[] includeProperties =
                {
            orderDto => orderDto
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Ticket)
                        .ThenInclude(t => t.Match)
                            .ThenInclude(m => m.TeamA),
            orderDto => orderDto
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Ticket)
                        .ThenInclude(t => t.Match)
                            .ThenInclude(m => m.TeamB)
        };

                // Filter to get only orders for the specified user
                Expression<Func<OrderDto, bool>> orderFilter = order => order.UserId == userId && order.CartItems.Any(ci => ci.Ticket != null);

                // Use the FindAsync method to get only relevant data
                var result = await FindAsync(orderFilter, useCache, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

                // Handle the result
                return result.Match(
                    orders =>
                    {
                        // Extract tickets from the cart items within the user’s orders
                        var tickets = orders.SelectMany(order => order.CartItems)
                                            .Where(cartItem => cartItem.Ticket != null)
                                            .Select(cartItem => cartItem.Ticket)
                                            .ToList();

                        // Map the tickets to DTOs
                        var ticketDtos = tickets.Select(ticket => new TicketDto
                        {
                            Id=ticket.Id,
                            Location = ticket.Location,
                            Class = ticket.Class,
                            TicketStatus = (TicketStatusDTO)ticket.TicketStatus,
                            Match = new MatchDto
                            {
                                Stadium=ticket.Match.Stadium,
                                EventDate = ticket.Match?.EventDate,
                                EventTime = ticket.Match?.EventTime,
                                TeamA = new TeamDto
                                {
                                    Name = ticket.Match?.TeamA?.Name,
                                    Logo=ticket.Match?.TeamA?.Logo
                                },
                                TeamB = new TeamDto
                                {
                                    Name = ticket.Match?.TeamB?.Name,
                                    Logo = ticket.Match?.TeamB?.Logo

                                }
                            }
                        }).ToList();

                        return new Result<IEnumerable<TicketDto>>(ticketDtos);
                    },
                    ex => new Result<IEnumerable<TicketDto>>(ex) // Handle failure case
                );
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<TicketDto>>(ex);
            }
        }

    }
}
