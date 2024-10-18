using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task CreateAsync(Ticket ticket, int count);
        void UpdateAllWithSamePredicate(Expression<Func<Ticket, bool>> predicate, Ticket ticket);
    }
}
