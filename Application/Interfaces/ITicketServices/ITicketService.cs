using Domain.Entities;
using Shared.Common;

namespace Application.Interfaces.ITicketServices
{
    public interface ITicketService
    {
        Task<BaseResponse<IEnumerable<Ticket?>>> GetDistinctTicketsAsync(long matchId);
        //Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default);
    }
}
