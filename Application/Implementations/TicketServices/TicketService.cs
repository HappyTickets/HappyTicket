using Application.Interfaces.ITicketServices;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common;

namespace Application.Implementations.TicketServices
{
    public class TicketService(
        IUnitOfWork unitOfWork,
        ILogger<Ticket> logger,
        IMapper mapper
        ) : BaseService<Ticket>(unitOfWork, logger, mapper), ITicketService
    {

        //public async Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        // Retrieve the ticket by its ID
        //        var ticketResult = await _unitOfWork.Repository<Ticket>().FirstOrDefaultAsync(
        //            x => x.Id == ticketId,
        //            cancellationToken
        //        );

        //        return await ticketResult.Match<Task<Result<string>>>(
        //            async ticket =>
        //            {
        //                // Check the current ticket status
        //                if (ticket.TicketStatus == TicketStatus.Sold || ticket.TicketStatus == TicketStatus.ForAdmins)
        //                {
        //                    // Update the status to "Used"
        //                    ticket.TicketStatus = TicketStatus.Used;

        //                    // Save the changes
        //                    _unitOfWork.Repository<Ticket>().Update(ticket);
        //                    await _unitOfWork.SaveChangesAsync(cancellationToken);

        //                    // Return success message
        //                    return new Result<string>(_localizer["Ticket_Scanned_Success1"]);
        //                }
        //                else
        //                {
        //                    // Return invalid operation if the ticket status is not appropriate
        //                    return new Result<string>(new InvalidOperationException(_localizer["Ticket_Status_Invalid1"]));
        //                }
        //            },
        //             async Fail =>
        //             {
        //                 await Task.Delay(0);
        //                 return new Result<string>(Fail);
        //             }
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Result<string>(ex);
        //    }
        //}
        public async Task<BaseResponse<IEnumerable<Ticket?>>> GetDistinctTicketsAsync(long matchTeamId)
        {
            var randomTicketsQuery = _unitOfWork.Repository<Ticket>().Query()
                .Where(t => t.MatchTeamId == matchTeamId && t.TicketStatus == Domain.Enums.TicketStatus.Active)
                .GroupBy(t => t.Class)
                .Select(g => g.OrderBy(t => Guid.NewGuid())
                .FirstOrDefault());

            return await randomTicketsQuery.ToListAsync();
        }
    }

}