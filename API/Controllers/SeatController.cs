using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Net;

namespace API.Controllers;

public class SeatController : BaseController
{
    private readonly ISeatService _seatService;

    public SeatController(IHttpContextAccessor httpContextAccessor, ISeatService seatService) : base(httpContextAccessor)
    {
        _seatService = seatService;
    }

    [HttpGet]
    [Route("GetSeats")]
    public async Task<ActionResult> GetSeats(bool useCache = true, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnListResult(await _seatService.GetAllAsync(useCache, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("GetSeat")]
    public async Task<ActionResult> GetSeatById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _seatService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("AddSeat")]
    public async Task<ActionResult> AddSeat([FromBody] SeatDto seat, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _seatService.CreateAsync(seat, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Route("EditSeat")]
    public async Task<ActionResult> EditSeat([FromBody] SeatDto seat, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _seatService.UpdateAsync(seat, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("RecoverSeat")]
    public async Task<ActionResult> RecoverSeatById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _seatService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("SoftDeleteSeat")]
    public async Task<ActionResult> SoftDeleteSeatById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _seatService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("HardDeleteSeat")]
    public async Task<ActionResult> HardDeleteSeatById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _seatService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}