using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Champion;

namespace API.Controllers
{
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
    }
}
