using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.DTOs.TicketDTOs;
using Application.Interfaces.ITicketServices;
using Microsoft.Extensions.Localization;
using Shared.ResourceFiles;
using Domain.Enums;
using LanguageExt.Common;

namespace Application.Implementations.TicketServices
{
    public class TicketService : BaseService<Ticket, TicketDto>, ITicketService
    {

        public TicketService(
            IUnitOfWork unitOfWork,
            ILogger<Ticket> logger,
            IMemoryCache cache,
            IMapper mapper,
            IValidator<TicketDto> validator,
            IStringLocalizer<Resource> localizer
        ) : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
        }

        public async Task<Result<string>> ScanQrCodeAsync(Guid ticketId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Retrieve the ticket by its ID
                var ticketResult = await _unitOfWork.Repository<Ticket>().FirstOrDefaultAsync(
                    x => x.Id == ticketId,
                    cancellationToken
                );

                return await ticketResult.Match<Task<Result<string>>>(
                    async ticket =>
                    {
                        // Check the current ticket status
                        if (ticket.TicketStatus == TicketStatus.Sold || ticket.TicketStatus == TicketStatus.ForAdmins)
                        {
                            // Update the status to "Used"
                            ticket.TicketStatus = TicketStatus.Used;

                            // Save the changes
                             _unitOfWork.Repository<Ticket>().Update(ticket);
                            await _unitOfWork.SaveChangesAsync(cancellationToken);

                            // Return success message
                            return new Result<string>(_localizer["Ticket_Scanned_Success1"]);
                        }
                        else
                        {
                            // Return invalid operation if the ticket status is not appropriate
                            return new Result<string>(new InvalidOperationException(_localizer["Ticket_Status_Invalid1"]));
                        }
                    },
                     async Fail =>
                     {
                         await Task.Delay(0);
                         return new Result<string>(Fail);
                     }
                );
            }
            catch (Exception ex)
            {
                return new Result<string>(ex);
            }
        }
    }

}
