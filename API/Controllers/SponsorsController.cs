using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Sponsors;

namespace API.Controllers
{
    public class SponsorsController : BaseController
    {
        private readonly ISponsorService _sponsorService;

        public SponsorsController(ISponsorService sponsorService)
        {
            _sponsorService = sponsorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(CreateOrUpdateSponsorDto request)
            => Result(await _sponsorService.CreateAsync(request));

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, CreateOrUpdateSponsorDto request)
            => Result(await _sponsorService.UpdateAsync(id, request));

        [HttpDelete("soft-delete/{id:long}")]
        public async Task<IActionResult> SoftDeleteAsync(long id)
            => Result(await _sponsorService.SoftDeleteAsync(id));

        [HttpDelete("hard-delete/{id:long}")]
        public async Task<IActionResult> HardDeleteAsync(long id)
            => Result(await _sponsorService.HardDeleteAsync(id));

        [HttpGet("get-by-id/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
           => Result(await _sponsorService.GetByIdAsync(id));

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
            => Result(await _sponsorService.GetAllAsync()); 
    }
}
