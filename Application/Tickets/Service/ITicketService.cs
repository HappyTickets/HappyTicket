using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.TicketDTOs;
using Shared.DTOs.Tickets;

namespace Application.Tickets.Service
{
    public interface ITicketService
    {
        Task<BaseResponse<Empty>> CreateAsync(CreateTicketsDto dto);
        Task<BaseResponse<Empty>> UpdateAsync(UpdateTicketsDto dto);
        Task<BaseResponse<IEnumerable<TicketDto>>> GetDistinctTicketsAsync(long matchId);
        Task<BaseResponse<List<RichTicketDto>>> GetMyTicketsAsync();

        //Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default);
    }
}