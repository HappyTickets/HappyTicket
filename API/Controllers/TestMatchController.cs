using Application.Tests.Matches.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.General;
using Shared.DTOs.Test.Request;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMatchController : ControllerBase
    {
        private readonly ITestMatchService _testMatchService;

        public TestMatchController(ITestMatchService testMatchService)
        {
            _testMatchService = testMatchService;
        }

        /* ******************************************************
            Follow Rest API  Naming convention in Route name

            Add Route names in ApiRoute as const string

           ******************************************************
         */

        // GET: api/testmatch
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _testMatchService.GetAll();
            return Ok(result);
        }

        // GET: api/testmatch/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _testMatchService.GetByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }


        [HttpGet("FindActiveMatches")]
        public async Task<IActionResult> FindActiveMatches()
        {

            var matches = await _testMatchService.FindActiveMatches();
            return Ok(matches);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationSearchModel paginationParams)
        {
            var result = await _testMatchService.GetPaginatedAsync(paginationParams);
            return Ok(result);
        }

        // POST: api/testmatch
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTestMatchDto matchDto)
        {
            var result = await _testMatchService.CreateAsyncTest(matchDto);
            if (result.IsSuccess)
            {
                NoContent();
            }
            return BadRequest(result);
        }

        // PUT: api/testmatch
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTestMatchDto matchDto)
        {
            var result = await _testMatchService.UpdateAsyncTest(matchDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        // PUT: api/testmatch
        [HttpPut("update-range")]
        public async Task<IActionResult> UpdateRange([FromBody] IEnumerable<UpdateTestMatchDto> matchDtos)
        {
            var result = await _testMatchService.UpdateRangeAsyncTest(matchDtos);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/testmatch/soft
        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDelete(long id)
        {
            var result = await _testMatchService.SoftDeleteByIdAsyncTest(id);
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
            var result = await _testMatchService.HardDeleteByIdAsyncTest(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result);
        }

        // GET: api/testmatch/recover/{id}
        [HttpGet("recover/{id}")]
        public async Task<IActionResult> RecoverById(long id)
        {
            var result = await _testMatchService.RecoverByIdAsyncTest(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }

}
