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
        public async Task<IActionResult> CreateAsync(CreateOrUpdateSponsorDto request, CancellationToken cancellationToken = default)
            => Result(await _sponsorService.CreateAsync(request, cancellationToken));

        [HttpPut("update/{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, CreateOrUpdateSponsorDto request, CancellationToken cancellationToken = default)
            => Result(await _sponsorService.UpdateAsync(id, request, cancellationToken));

        [HttpDelete("soft-delete/{id:long}")]
        public async Task<IActionResult> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
            => Result(await _sponsorService.SoftDeleteAsync(id, cancellationToken));

        [HttpDelete("hard-delete/{id:long}")]
        public async Task<IActionResult> HardDeleteAsync(long id, CancellationToken cancellationToken = default)
            => Result(await _sponsorService.HardDeleteAsync(id, cancellationToken));

        [HttpGet("get-by-id/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id, CancellationToken cancellationToken = default)
           => Result(await _sponsorService.GetByIdAsync(id, cancellationToken));

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
            => Result(await _sponsorService.GetAllAsync(cancellationToken)); 
    }
}
