﻿using Application.Interfaces.ITicketServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.DTOs.TicketDTOs;
using Shared.Enums;
using System.Net;

namespace API.Controllers
{
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;

        public TicketController(IHttpContextAccessor httpContextAccessor, ITicketService ticketService) : base(httpContextAccessor)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        [Route("GetTickets")]
        public async Task<ActionResult> GetTickets(bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _ticketService.GetAllAsync(useCache, cancellationToken: cancellationToken);
                return ReturnListResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetTicketsByMatchId")]
        public async Task<ActionResult> GetTicketsByMatchId(Guid id, Guid? selectedTeamId, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _ticketService.FindAsync(x => x.MatchId == id && x.TeamId == selectedTeamId, useCache, cancellationToken: cancellationToken);
                return ReturnListResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("GetTicketsByMatchIdAndSelectedTeam")]
        public async Task<ActionResult> GetTicketsByMatchIdAndSelectedTeam(Guid id, Guid? selectedTeamId, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _ticketService.FindAsync(x => x.MatchId == id && x.TeamId == selectedTeamId && (x.TicketStatus == TicketStatusDTO.Active || x.TicketStatus == TicketStatusDTO.Incart), useCache, cancellationToken: cancellationToken);
                return ReturnListResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTicket")]
        public async Task<ActionResult> GetTicketById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _ticketService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("AddTickets")]
        public async Task<ActionResult> AddTickets([FromBody] List<TicketDto> tickets, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _ticketService.CreateRangeAsync(tickets, cancellationToken: cancellationToken);
                    return ReturnResult(result);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost("updateTickets")]
        public async Task<IActionResult> UpdateTickets([FromBody] List<TicketDto> tickets, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _ticketService.UpdateRangeAsync(tickets, cancellationToken: cancellationToken);
                    return ReturnResult(result);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("EditTicket")]
        public async Task<ActionResult> EditTicket([FromBody] TicketDto ticket, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _ticketService.UpdateAsync(ticket, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("RecoverTicket")]
        public async Task<ActionResult> RecoverTicketById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _ticketService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("SoftDeleteTicket")]
        public async Task<ActionResult> SoftDeleteTicketById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _ticketService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("HardDeleteTicket")]
        public async Task<ActionResult> HardDeleteTicketById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _ticketService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("HardDeleteTicketByMatch")]
        public async Task<ActionResult> HardDeleteTicketByMatch(Guid matchId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _ticketService.HardDeleteRangeAsync(x => x.MatchId == matchId, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("ScanTicketQr")]
        public async Task<ActionResult> ScanTicketQr([FromBody] Guid ticketId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _ticketService.ScanQrCodeAsync(ticketId, cancellationToken);

                // Use Match to handle success and failure cases, explicitly setting ActionResult as the return type
                var res = result.Match<ActionResult>(
                    success => Ok(new BaseResponse<string>(success)), // Wrap success in BaseResponse<string>
                    failure => BadRequest(new BaseResponse<string> { Title = "Error", ErrorList = new[] { new ResponseError("Error", failure.Message) } })
                );
                return res;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
