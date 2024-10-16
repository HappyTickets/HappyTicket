//using Application.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using Shared.DTOs.Champion;
//using System.Net;

//namespace API.Controllers
//{
//    public class ChampionController : BaseController
//    {
//        private readonly IChampionService _championService;
//        public ChampionController(IHttpContextAccessor httpContextAccessor, IChampionService championService) : base(httpContextAccessor)
//        {
//            _championService = championService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _championService.GetAllAsync(useCache: useCache, cancellationToken: cancellationToken));
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
//                    return ReturnResult(await _championService.GetByIdAsync(id, useCache: useCache, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateAsync(CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _championService.CreateAsync(dto, cancellationToken: cancellationToken));
//                }

//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
//            }
//        }

//        [HttpPut("{id:guid}")]
//        public async Task<IActionResult> UpdateAsync(Guid id, CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    return ReturnResult(await _championService.UpdateAsync(id, dto, cancellationToken: cancellationToken));
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
//                    return ReturnResult(await _championService.DeleteAsync(id, cancellationToken: cancellationToken));
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
