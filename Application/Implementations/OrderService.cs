using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Enums;
using Shared.Common.General;
using Shared.DTOs;
using Shared.DTOs.Identity.UserDTOs;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.Team;
using Shared.DTOs.TicketDTOs;
using Shared.ResourceFiles;
using System.Globalization;
using System.Linq.Expressions;

namespace Application.Implementations
{
    public class OrderService : BaseService<Order, OrderDto>, IOrderService
    {
        public OrderService(IUnitOfWork unitOfWork, ILogger<Order> logger, IMemoryCache cache, IMapper mapper, IValidator<OrderDto> validator, IStringLocalizer<Resource> localizer)
            : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
        }
        //public async Task<Result<IEnumerable<OrderDto>>> GetPaginatedOrdersAsync(PaginationParams paginationParams, bool useCache = false, Func<Order, OrderDto>? customMapper = null, CancellationToken cancellationToken = default)
        //{

        //    try
        //    {
        //        Expression<Func<IQueryable<OrderDto>, IIncludableQueryable<OrderDto, object>>>[] includeProperties =
        //        {
        //            x=>x.Include(o=>o.User),

        //        };
        //        Func<Order, OrderDto>? orderMap = order => new OrderDto
        //        {
        //            User = new ApplicationUserDTO
        //            {
        //                UserName = order.User.UserName,
        //                Email = order.User.Email,
        //                PhoneNumber = order.User.PhoneNumber,
        //            },
        //            TotalAmount = order.TotalAmount,
        //            PaymentStatus = order.PaymentStatus,
        //            PaymentStatusString = order.PaymentStatus.HasValue ? ((PaymentStatusEnum)order.PaymentStatus.Value).ToString() : null,
        //            CreatedDate = order.CreatedDate,
        //            ModifiedBy = order.ModifiedBy,

        //        };

        //        var result = await GetPaginatedAsync(paginationParams, useCache, orderMap, cancellationToken: cancellationToken, includeDTOProperties: includeProperties);

        //        return result;
        //    }


        //    catch (Exception ex)
        //    {
        //        return new Result<IEnumerable<OrderDto>>(ex);
        //    }

        //}
        public async Task<Result<IEnumerable<OrderDto>>> GetPaginatedOrdersAsync(
                     PaginationSearchModel paginationSearchModel,
                     bool useCache = false,
                     Func<Order, OrderDto>? customMapper = null,
                     CancellationToken cancellationToken = default)
        {

            try
            {
                // Define the properties to include in the user
                //aslo avoid multiple database calls and retrieve related data in a single query
                Expression<Func<IQueryable<OrderDto>, IIncludableQueryable<OrderDto, object>>>[] includeProperties =
                {
                    x => x.Include(o => o.User),
                };

                // Create a mapping function for converting Order to OrderDto
                Func<Order, OrderDto> orderMap = order => new OrderDto
                {
                    User = new ApplicationUserDTO
                    {
                        UserName = order.User.UserName,
                        Email = order.User.Email,
                        PhoneNumber = order.User.PhoneNumber,
                    },
                    TotalAmount = order.TotalAmount,
                    PaymentStatus = order.PaymentStatus,
                    PaymentStatusString = order.PaymentStatus.HasValue
                        ? ((PaymentStatusEnum)order.PaymentStatus.Value).ToString()//converting the number value of enum to string value
                        : null,
                    CreatedDate = order.CreatedDate,
                    ModifiedBy = order.ModifiedBy,
                };

                // Get the queryable for orders and include related entities to do the sort logic
                var query = _unitOfWork.Repository<Order>().Query();
                query = query.Include(o => o.User);
                //filtering logic
                if (!string.IsNullOrEmpty(paginationSearchModel.SearchKey) && !string.IsNullOrEmpty(paginationSearchModel.SearchIn))
                {
                    switch (paginationSearchModel.SearchIn.ToLower())
                    {
                        case "username":
                            query = query.Where(o => o.User.UserName.Contains(paginationSearchModel.SearchKey));
                            break;
                        case "useremail":
                            query = query.Where(o => o.User.Email.Contains(paginationSearchModel.SearchKey));
                            break;
                        case "phonenumber":
                            query = query.Where(o => o.User.PhoneNumber.Contains(paginationSearchModel.SearchKey));
                            break;
                        case "paymentstatus":
                            query = query.Where(o => ((PaymentStatusEnum)o.PaymentStatus.Value).ToString().ToLower().Contains(paginationSearchModel.SearchKey));
                            //query = query.AsEnumerable() // Moves the query to be executed in memory
                            //             .Where(o => ((PaymentStatusEnum)o.PaymentStatus).ToString().ToLower()
                            //             .Contains(paginationSearchModel.SearchKey.ToLower())).AsQueryable();
                            break;
                        case "createddate":

                            query = query.Where(o => o.CreatedDate.ToString().Contains(paginationSearchModel.SearchKey));

                            break;

                        case "modifiedby":
                            query = query.Where(o => o.ModifiedBy.Equals(paginationSearchModel.SearchKey));
                            break;
                        case "totalpaid":

                            query = query.Where(o => o.TotalAmount.ToString().Contains(paginationSearchModel.SearchKey.ToString()));

                            break;

                    }
                }

                // Sorting logic
                if (!string.IsNullOrEmpty(paginationSearchModel.OrderBy))
                {
                    var sortExpression = GetSortExpression(paginationSearchModel.OrderBy);
                    query = paginationSearchModel.IsDescending
                        ? query.OrderByDescending(sortExpression)
                        : query.OrderBy(sortExpression);
                }

                // Count total records for pagination
                int totalCount = await query.CountAsync();

                //Apply pagination
                //var orders = await query
                //    .Skip(paginationSearchModel.PageIndex * 10)
                //    .Take(10)
                //    .ToListAsync();
                var orders = await query
                    .Skip(paginationSearchModel.PageIndex * paginationSearchModel.PageSize)
                    .Take(paginationSearchModel.PageSize)
                    .ToListAsync();

                // Map orders to DTOs
                var orderDtos = orders.Select(orderMap).ToList();

                // Return result
                return new Result<IEnumerable<OrderDto>>(orderDtos);
            }
            catch (Exception ex)
            {
                return new Result<IEnumerable<OrderDto>>(ex);
            }
        }

        //function for creating sorting expressions based on the provided field name
        private Expression<Func<Order, object>> GetSortExpression(string sortBy)
        {
            return sortBy switch
            {
                "UserName" => o => o.User.UserName,
                "Email" => o => o.User.Email,
                "TotalAmount" => o => o.TotalAmount,
                "PaymentStatus" => o => o.PaymentStatus,
                "CreatedDate" => o => o.CreatedDate,
                "ModifiedBy" => o => o.ModifiedBy,
                _ => o => o.Id
            };
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
                            .ThenInclude(m => m.TeamB),
            orderDto => orderDto
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Ticket)
                        .ThenInclude(t => t.Match)
                            .ThenInclude(m => m.Stadium),
            orderDto => orderDto
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Ticket)
                        .ThenInclude(t => t.Match)
                            .ThenInclude(m => m.Champion)
                            .ThenInclude(m => m.ChampionSponsors)
                            .ThenInclude(m => m.Sponsor)

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
                            Id = ticket.Id,
                            Location = ticket.Location,
                            Class = ticket.Class,
                            TicketStatus = ticket.TicketStatus,
                            TeamId = ticket.TeamId,
                            ExternalGate = ticket.ExternalGate,
                            InternalGate = ticket.InternalGate,

                            Match = new MatchDto
                            {
                                Stadium = ticket.Match.Stadium,
                                EventDate = ticket.Match?.EventDate,
                                EventTime = ticket.Match?.EventTime,
                                TeamAId = ticket.Match.TeamAId,
                                Champion = ticket.Match.Champion,
                                TeamBId = ticket.Match.TeamBId,
                                TeamA = new TeamDto
                                {
                                    Name = ticket.Match?.TeamA?.Name,
                                    Logo = ticket.Match?.TeamA?.Logo
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
