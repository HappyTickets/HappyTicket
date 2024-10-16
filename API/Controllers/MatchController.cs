//using Application.Implementations;
//using Application.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Shared.DTOs.MatchDtos;
//using System.Net;

//namespace API.Controllers;

//public class MatchController : BaseController
//{
//    private readonly IMatchService _matchService;
//    private readonly IMatchCommandService _matchCommandService;

//    public MatchController(IHttpContextAccessor httpContextAccessor, IMatchService matchService, IMatchCommandService matchCommandService) : base(httpContextAccessor)
//    {
//        _matchService = matchService;
//        _matchCommandService = matchCommandService;
//    }

//    [HttpGet]
//    [Route("GetMatches")]
//    public async Task<ActionResult> GetMatches(bool useCache = true, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var result = await _matchService.GetMatchesAsync(useCache, cancellationToken);
//            return ReturnListResult(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }
//    [HttpGet]
//    [Route("GetActiveMatches")]
//    public async Task<ActionResult> GetActiveMatches(bool useCache = true, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            var result = await _matchService.GetActiveMatchesAsync(useCache, cancellationToken);
//            return ReturnListResult(result);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }


//    [HttpGet]
//    [Route("GetMatch")]
//    public async Task<ActionResult> GetMatchById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                var result = ReturnResult(await _matchService.GetMatchByIdAsync(id, useCache, cancellationToken: cancellationToken));
//                return result;
//            }

//            return BadRequest(ModelState);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }

//    [HttpPost]
//    [Route("AddMatch")]
//    public async Task<ActionResult> AddMatch([FromBody] MatchCommandDto match, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await _matchCommandService.CreateAsync(match, cancellationToken: cancellationToken);
//                return ReturnResult(result);
//            }

//            return BadRequest(ModelState);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }

//    [HttpPut]
//    [Route("UpdateMatch")]
//    public async Task<ActionResult> EditMatch([FromBody] MatchDto match, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                return ReturnResult(await _matchService.UpdateAsync(match, cancellationToken: cancellationToken));
//            }

//            return BadRequest(ModelState);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }

//    [HttpGet]
//    [Route("RecoverMatch")]
//    public async Task<ActionResult> RecoverMatchById(Guid id, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                return ReturnResult(await _matchService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
//            }

//            return BadRequest(ModelState);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }

//    [HttpGet]
//    [Route("SoftDeleteMatch")]
//    public async Task<ActionResult> SoftDeleteMatchById(Guid id, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                return ReturnResult(await _matchService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
//            }

//            return BadRequest(ModelState);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }

//    [HttpGet]
//    [Route("HardDeleteMatch")]
//    public async Task<ActionResult> HardDeleteMatchById(Guid id, CancellationToken cancellationToken = default)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                return ReturnResult(await _matchService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
//            }

//            return BadRequest(ModelState);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//        }
//    }
//}
