using Domain.Entities;
using LanguageExt.Common;
using Shared.DTOs.TicketDTOs;

namespace Application.Interfaces.ITicketServices
{
    public interface ITicketService : IBaseService<Ticket, TicketDto>
    {
        Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default);
    }
}
