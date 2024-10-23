using Application.Tickets.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.TicketDTOs;

namespace API.Controllers
{
    public class TicketsController(ITicketService ticketService) : BaseController
    {
        private readonly ITicketService _ticketService = ticketService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateTicketsDto dto)
            => Result(await _ticketService.CreateAsync(dto));

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(UpdateTicketsDto dto)
            => Result(await _ticketService.UpdateAsync(dto));

        [HttpGet]
        [Route("GetRandomTickets")]
        public async Task<ActionResult> GetRandomTickets(long matchTeamId, CancellationToken cancellationToken = default)
        {
            var result = await _ticketService.GetDistinctTicketsAsync(matchTeamId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetMyTickets")]
        [Authorize]
        public async Task<ActionResult> GetMyTickets()
        {

            var result = await _ticketService.GetMyTicketsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        //[HttpGet]
        //[Route("GetTickets")]
        //public async Task<ActionResult> GetTickets(bool useCache = true, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var result = await _ticketService.GetAllAsync(useCache, cancellationToken: cancellationToken);
        //        return ReturnListResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //[HttpGet]
        //[Route("GetTicketsByMatchId")]
        //public async Task<ActionResult> GetTicketsByMatchId(Guid id, Guid? selectedTeamId, bool useCache = true, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var result = await _ticketService.FindAsync(x => x.MatchId == id && x.TeamId == selectedTeamId, useCache, cancellationToken: cancellationToken);
        //        return ReturnListResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //[HttpGet]
        //[Route("GetTicketsByMatchIdAndSelectedTeam")]
        //public async Task<ActionResult> GetTicketsByMatchIdAndSelectedTeam(Guid id, Guid? selectedTeamId, bool useCache = true, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var result = await _ticketService.FindAsync(x => x.MatchId == id && x.TeamId == selectedTeamId && (x.TicketStatus == TicketStatusDTO.Active || x.TicketStatus == TicketStatusDTO.Incart), useCache, cancellationToken: cancellationToken);
        //        return ReturnListResult(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("GetTicket")]
        //public async Task<ActionResult> GetTicketById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            return ReturnResult(await _ticketService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //[HttpPost]
        //[Route("AddTickets")]
        //public async Task<ActionResult> AddTickets([FromBody] List<TicketDto> tickets, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var result = await _ticketService.CreateRangeAsync(tickets, cancellationToken: cancellationToken);
        //            return ReturnResult(result);
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //[HttpPost("updateTickets")]
        //public async Task<IActionResult> UpdateTickets([FromBody] List<TicketDto> tickets, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var result = await _ticketService.UpdateRangeAsync(tickets, cancellationToken: cancellationToken);
        //            return ReturnResult(result);
        //        }
        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPut]
        //[Route("EditTicket")]
        //public async Task<ActionResult> EditTicket([FromBody] TicketDto ticket, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            return ReturnResult(await _ticketService.UpdateAsync(ticket, cancellationToken: cancellationToken));
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("RecoverTicket")]
        //public async Task<ActionResult> RecoverTicketById(Guid id, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            return ReturnResult(await _ticketService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("SoftDeleteTicket")]
        //public async Task<ActionResult> SoftDeleteTicketById(Guid id, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            return ReturnResult(await _ticketService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("HardDeleteTicket")]
        //public async Task<ActionResult> HardDeleteTicketById(Guid id, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            return ReturnResult(await _ticketService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("HardDeleteTicketByMatch")]
        //public async Task<ActionResult> HardDeleteTicketByMatch(Guid matchId, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            return ReturnResult(await _ticketService.HardDeleteRangeAsync(x => x.MatchId == matchId, cancellationToken: cancellationToken));
        //        }

        //        return BadRequest(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("ScanTicketQr")]
        //public async Task<ActionResult> ScanTicketQr([FromBody] Guid ticketId, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var result = await _ticketService.ScanQrCodeAsync(ticketId, cancellationToken);

        //        // Use Match to handle success and failure cases, explicitly setting ActionResult as the return type
        //        var res = result.Match<ActionResult>(
        //            success => Ok(new BaseResponse<string>(success)), // Wrap success in BaseResponse<string>
        //            failure => BadRequest(new BaseResponse<string> { Title = "Error", ErrorList = new[] { new ResponseError("Error", failure.Message) } })
        //        );
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

    }
}
