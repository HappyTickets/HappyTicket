using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Net;

namespace API.Controllers
{
    public class StadiumController : BaseController
    {
        private readonly IStadiumService _stadiumService;

        public StadiumController(IHttpContextAccessor httpContextAccessor, IStadiumService stadiumService) : base(httpContextAccessor)
        {
            _stadiumService = stadiumService;
        }

        [HttpGet]
        [Route("GetStadiums")]
        public async Task<ActionResult> GetStadiums(bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return ReturnListResult(await _stadiumService.GetAllAsync(useCache, cancellationToken: cancellationToken));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetStadium")]
        public async Task<ActionResult> GetStadiumById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _stadiumService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddStadium")]
        public async Task<ActionResult> AddStadium([FromBody] StadiumDto stadium, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _stadiumService.CreateAsync(stadium, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("EditStadium")]
        public async Task<ActionResult> EditStadium([FromBody] StadiumDto stadium, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _stadiumService.UpdateAsync(stadium, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        
        [HttpGet]
        [Route("Delete")]
        public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _stadiumService.DeleteStadiumWithNoMatchesAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
