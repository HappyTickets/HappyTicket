﻿using Domain.Entities;
using Shared.Common.General;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<PaginatedList<Ticket>> GetMyTicketsAsync(long userId, PaginationParams pagination);
        Task CreateAsync(Ticket ticket, int count);
        void UpdateAllWithSamePredicate(Expression<Func<Ticket, bool>> predicate, Ticket ticket);

    }
}
