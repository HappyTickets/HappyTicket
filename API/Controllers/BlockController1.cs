using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Net;

namespace API.Controllers;

public class BlockController : BaseController
{
    private readonly IBlockService _blockService;

    public BlockController(IHttpContextAccessor httpContextAccessor, IBlockService blockService) : base(httpContextAccessor)
    {
        _blockService = blockService;
    }

    [HttpGet]
    [Route("GetBlocks")]
    public async Task<ActionResult> GetBlocks(bool useCache = true, CancellationToken cancellationToken = default)
    {
        try
        {
            return ReturnListResult(await _blockService.GetAllAsync(useCache, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("GetBlock")]
    public async Task<ActionResult> GetBlockById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _blockService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("AddBlock")]
    public async Task<ActionResult> AddBlock([FromBody] BlockDto block, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _blockService.CreateAsync(block, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    [Route("EditBlock")]
    public async Task<ActionResult> EditBlock([FromBody] BlockDto block, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _blockService.UpdateAsync(block, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("RecoverBlock")]
    public async Task<ActionResult> RecoverBlockById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _blockService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("SoftDeleteBlock")]
    public async Task<ActionResult> SoftDeleteBlockById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _blockService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    [Route("HardDeleteBlock")]
    public async Task<ActionResult> HardDeleteBlockById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (ModelState.IsValid)
            {
                return ReturnResult(await _blockService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
