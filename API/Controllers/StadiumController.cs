using Application.Stadiums.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.StadiumDTO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController(IStadiumService _stadiumService) : ControllerBase
    {

        [HttpPost("Create_Stadiums")]
        public async Task<IActionResult> CreateStadium([FromBody] CreateStadiumDto stadiumDto)
        {
            var result = await _stadiumService.CreateStadiumAsync(stadiumDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Update_stadium")]
        public async Task<IActionResult> UpdateStadium([FromBody] UpdateStadiumDto stadiumDto)
        {
            var result = await _stadiumService.UpdateAsync(stadiumDto);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound();
        }
        [HttpGet("GetAll_StadiumsAsync")]
        public async Task<IActionResult> GetStadiums()
        {
            return Ok(await _stadiumService.GetStadiums());
        }
        [HttpGet("Get_StaduimByIdAsync")]
        public async Task<IActionResult> GetStaduimById(long stadiumId)
        {
            return Ok(await _stadiumService.GetStaduimByIdAsync(stadiumId));
        }

        [HttpDelete("HardDeleteStadium")]
        public async Task<IActionResult> DeleteStadium(long stadiumId)
        {
            var result = await _stadiumService.DeleteStadiumAsync(stadiumId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
