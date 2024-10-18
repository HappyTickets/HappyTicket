using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController(IStadiumService _stadiumService) : ControllerBase
    {

        [HttpGet("GetAll_Stadiums")]
        public async Task<IActionResult> GetStadiums()
        {
            return Ok(await _stadiumService.GetStadiums());
        }
        [HttpGet("Get_StaduimById")]
        public async Task<IActionResult> GetStaduimById(long stadiumId)
        {
            return Ok(await _stadiumService.GetStaduimById(stadiumId));
        }

        //        [HttpGet]
        //        [Route("GetStadium")]
        //        public async Task<ActionResult> GetStadiumById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        [HttpPost]
        //        [Route("AddStadium")]
        //        public async Task<ActionResult> AddStadium([FromBody] StadiumDto stadium, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.CreateAsync(stadium, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        [HttpPut]
        //        [Route("EditStadium")]
        //        public async Task<ActionResult> EditStadium([FromBody] StadiumDto stadium, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.UpdateAsync(stadium, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        [HttpGet]
        //        [Route("RecoverStadium")]
        //        public async Task<ActionResult> RecoverStadiumById(Guid id, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        [HttpGet]
        //        [Route("SoftDeleteStadium")]
        //        public async Task<ActionResult> SoftDeleteStadiumById(Guid id, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        [HttpGet]
        //        [Route("HardDeleteStadium")]
        //        public async Task<ActionResult> HardDeleteStadiumById(Guid id, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        } 

        //        [HttpGet]
        //        [Route("DeleteStadiumWithNoMatches")]
        //        public async Task<ActionResult> DeleteStadiumWithNoMatches(Guid id, CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    return ReturnResult(await _stadiumService.DeleteStadiumWithNoMatchesAsync(id, cancellationToken: cancellationToken));
        //                }

        //                return BadRequest(ModelState);
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }
    }
}
