﻿using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.TicketDTOs;
using System.Net;

namespace Application.Tickets.Service
{
    public class TicketService(
        IUnitOfWork unitOfWork,
        ILogger<Ticket> logger,
        IMapper mapper
        ): BaseService<Ticket>(unitOfWork, logger, mapper), ITicketService
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

        public async Task<BaseResponse<object?>> CreateAsync(CreateTicketsDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);
            await _unitOfWork.Tickets.CreateAsync(ticket, dto.TicketsCount);

            return HttpStatusCode.Created;
        }

        public async Task<BaseResponse<object?>> UpdateAsync(UpdateTicketsDto dto)
        {
            var ticket = _mapper.Map<Ticket>(dto);

            _unitOfWork.Tickets.UpdateAllWithSamePredicate(t =>
                t.MatchTeamId == dto.OldMatchTeamId &&
                t.Class == dto.OldClass &&
                t.Price == dto.OldPrice &&
                t.Notes == dto.OldNotes &&
                t.BlockId == dto.OldBlockId &&
                t.SeatId == dto.OldSeatId &&
                t.DisplayForSale == dto.OldDisplayForSale &&
                t.Location == dto.OldLocation &&
                t.TicketStatus == Enum.Parse<TicketStatus>(dto.OldTicketStatus.ToString()) &&
                t.SeatNumber == dto.OldSeatNumber &&
                t.ExternalGate == dto.OldExternalGate &&
                t.InternalGate == dto.OldInternalGate,
                ticket);

            await _unitOfWork.SaveChangesAsync();

            return HttpStatusCode.OK;
        }

        public async Task<BaseResponse<IEnumerable<Ticket?>>> GetDistinctTicketsAsync(long matchTeamId)
        {
            var randomTicketsQuery = _unitOfWork.Repository<Ticket>().Query()
                .Where(t => t.MatchTeamId == matchTeamId && t.TicketStatus == TicketStatus.Active)
                .GroupBy(t => t.Class)
                .Select(g => g.OrderBy(t => Guid.NewGuid())
                .FirstOrDefault());

            return await randomTicketsQuery.ToListAsync();
        }
    }

}