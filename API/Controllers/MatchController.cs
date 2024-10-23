using Application.Implementations;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.General;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.Test.Request;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _matchService.GetAll();
            return Ok(result);
        }

        // GET: api/testmatch/{id}
        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _matchService.GetMatchByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }


        [HttpGet("GetActiveMatches")]
        public async Task<IActionResult> GetActiveMatches()
        {

            var matches = await _matchService.FindActiveMatches();
            return Ok(matches);
        }

        [HttpGet("GetPaginatedList")]
        public async Task<IActionResult> GetPaginatedList([FromQuery] PaginationSearchModel paginationParams)
        {
            var result = await _matchService.GetPaginatedAsync(paginationParams);
            return Ok(result);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateMatchDto matchDto)
        {
            var result = await _matchService.CreateMatchAsync(matchDto);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateMatchDto matchDto, long id)
        {
            var result = await _matchService.UpdateMatchAsync(matchDto, id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        // PUT: api/testmatch
        [HttpPut("update-range")]
        public async Task<IActionResult> UpdateRange([FromBody] IEnumerable<UpdateMatchDto> matchDtos)
        {
            var result = await _matchService.UpdateRangeAsync(matchDtos);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/testmatch/soft
        [HttpDelete("softDelete/{id}")]
        public async Task<IActionResult> SoftDelete(long id)
        {
            var result = await _matchService.SoftDeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result);
        }

        // DELETE: api/testmatch/hard
        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDelete(long id)
        {
            var result = await _matchService.HardDeleteByIdAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result);
        }

        // GET: api/testmatch/recover/{id}
        [HttpGet("RecoverById/{id}")]
        public async Task<IActionResult> RecoverById(long id)
        {
            var result = await _matchService.RecoverByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }

}























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
