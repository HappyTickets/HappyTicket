using Domain.Entities;
using Shared.Common;
using Shared.DTOs.TicketDTOs;

namespace Application.Tickets.Service
{
    public interface ITicketService
    {
        Task<BaseResponse<object?>> CreateAsync(CreateTicketsDto dto);
        Task<BaseResponse<object?>> UpdateAsync(UpdateTicketsDto dto);

        Task<BaseResponse<IEnumerable<Ticket?>>> GetDistinctTicketsAsync(long matchId);
        //Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default);
    }
}
