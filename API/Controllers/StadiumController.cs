using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.StadiumDTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController(IStadiumService _stadiumService) : ControllerBase
    {

        [HttpPost("Create")]
        public async Task<IActionResult> CreateStadium([FromBody] CreateStadiumDto stadiumDto)
        {
            var result = await _stadiumService.CreateStadiumAsync(stadiumDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateStadium([FromBody] UpdateStadiumDto stadiumDto)
        {
            var result = await _stadiumService.UpdateAsync(stadiumDto);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound();
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetStadiums()
        {
            return Ok(await _stadiumService.GetStadiums());
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetStaduimById(long stadiumId)
        {
            return Ok(await _stadiumService.GetStaduimByIdAsync(stadiumId));
        }

        [HttpDelete("HardDelete")]
        public async Task<IActionResult> DeleteStadium(long stadiumId)
        {
            var result = await _stadiumService.DeleteStadiumAsync(stadiumId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
