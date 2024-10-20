using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Champion;
using Shared.DTOs.ChampionDtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChampionController(IChampionService _championService) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> CreateChampionship([FromBody] CreateChampionshipDto createChampionshipDto)
        {
            var result = await _championService.CreateChampionAsync(createChampionshipDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateChamiponship([FromBody] UpdateChampionshipDto updateChampionshipDto)
        {
            var result = await _championService.UpdateAsync(updateChampionshipDto);
            if (result.IsSuccess)
                return Ok(result);
            return NotFound();
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllChampion()
        {
            return Ok(await _championService.GetAllChampionshipsAsync());
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetChampionshipById(long championshipId)
        {
            return Ok(await _championService.GetChampionshipByIdAsync(championshipId));
        }
        [HttpDelete("HardDelete")]
        public async Task<IActionResult> DeleteStadium(long championshipId)
        {
            var result = await _championService.DeleteChampionAsync(championshipId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
