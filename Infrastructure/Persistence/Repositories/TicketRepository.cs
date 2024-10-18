using Application.Interfaces.Infrastructure.Persistence;
using Application.Interfaces.Infrastructure.Services;
using Domain.Entities;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    internal class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly ICurrentUser _currentUser;

        public TicketRepository(AppDbContext context, ICurrentUser currentUser) : base(context)
        {
            _currentUser = currentUser;
        }

        public Task CreateAsync(Ticket ticket, int count)
        {
            return _dbContext.Database.ExecuteSqlRawAsync("EXEC CreateTickets @matchTeamId, @price, @notes, @blockId, @seatId, @displayForSale, @location, @class, @ticketStatus, @seatNumber, @externalGate, @internalGate, @baseEntityStatus, @createdBy, @createdDate, @modifiedBy, @modifiedDate, @isActive, @softDeleteCount, @ticketsCount",
                CreateParameter("@matchTeamId", ticket.MatchTeamId),
                CreateParameter("@price", ticket.Price),
                CreateParameter("@notes", ticket.Notes),
                CreateParameter("@blockId", ticket.BlockId),
                CreateParameter("@seatId", ticket.SeatId),
                CreateParameter("@displayForSale", ticket.DisplayForSale),
                CreateParameter("@location", ticket.Location),
                CreateParameter("@class", ticket.Class),
                CreateParameter("@ticketStatus", ticket.TicketStatus),
                CreateParameter("@seatNumber", ticket.SeatNumber),
                CreateParameter("@externalGate", ticket.ExternalGate),
                CreateParameter("@internalGate", ticket.InternalGate),
                CreateParameter("@baseEntityStatus", ticket.BaseEntityStatus),
                CreateParameter("@createdBy", _currentUser.Id),
                CreateParameter("@createdDate", DateTime.UtcNow),
                CreateParameter("@modifiedBy", null),
                CreateParameter("@modifiedDate", null),
                CreateParameter("@isActive", ticket.IsActive),
                CreateParameter("@softDeleteCount", ticket.SoftDeleteCount),
                CreateParameter("@ticketsCount", count)
                );
        }

        public void UpdateAllWithSamePredicate(Expression<Func<Ticket, bool>> predicate, Ticket ticket)
        {
            _dbContext.Tickets.Where(predicate)
                .ExecuteUpdate(s => s
                    .SetProperty(t => t.MatchTeamId, ticket.MatchTeamId)
                    .SetProperty(t => t.Price, ticket.Price)
                    .SetProperty(t => t.Notes, ticket.Notes)
                    .SetProperty(t => t.BlockId, ticket.BlockId)
                    .SetProperty(t => t.SeatId, ticket.SeatId)
                    .SetProperty(t => t.DisplayForSale, ticket.DisplayForSale)
                    .SetProperty(t => t.Location, ticket.Location)
                    .SetProperty(t => t.Class, ticket.Class)
                    .SetProperty(t => t.TicketStatus, ticket.TicketStatus)
                    .SetProperty(t => t.SeatNumber, ticket.SeatNumber)
                    .SetProperty(t => t.ExternalGate, ticket.ExternalGate)
                    .SetProperty(t => t.InternalGate, ticket.InternalGate)
                    .SetProperty(t => t.ModifiedBy, _currentUser.Id)
                    .SetProperty(t => t.ModifiedDate, DateTime.UtcNow)
                );
               
        }

        private SqlParameter CreateParameter(string name, object? value)
            => new SqlParameter(name, value ?? DBNull.Value);
    }
}
