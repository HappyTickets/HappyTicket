//using Application.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Shared.DTOs;
//using System.Net;

//namespace API.Controllers
//{
//    public class SponsorController : BaseController
//    {
//        private readonly ISponsorService _sponsorService;

//        public SponsorController(IHttpContextAccessor httpContextAccessor, ISponsorService sponsorService) : base(httpContextAccessor)
//        {
//            _sponsorService = sponsorService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _sponsorService.GetAllAsync(useCache: useCache, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpGet("{id:guid}")]
//        public async Task<IActionResult> GetByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _sponsorService.GetByIdAsync(id, useCache: useCache, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateAsync(SponsorDto dto, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _sponsorService.CreateAsync(dto, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateAsync(SponsorDto dto, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _sponsorService.UpdateAsync(dto, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpDelete("{id:guid}")]
//        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _sponsorService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }
//    }
//}
