using Application.Interfaces;
using Application.Interfaces.Persistence;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.DTOs.Team;
using System.Net;

namespace API.Controllers
{
    public class TeamController : BaseController
    {
        private readonly ITeamService _teamService;
        private readonly IUnitOfWork unitOfWork;

        public TeamController(IHttpContextAccessor httpContextAccessor, ITeamService teamService, IUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _teamService = teamService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("AddFavoriteTeam")]
        public async Task<ActionResult> AddFavoriteTeam(UserFavoriteTeamDto userFavoriteTeam, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _teamService.AddFavTeam(userFavoriteTeam, cancellationToken);
                    return ReturnResult(result);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("RemoveFavoriteTeam")]
        public async Task<ActionResult> RemoveFavoriteTeam(UserFavoriteTeamDto userFavoriteTeam, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _teamService.RemoveFavTeam(userFavoriteTeam, cancellationToken);
                    return ReturnResult(result);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserFavoriteTeam")]
        public async Task<ActionResult> GetUserFavoriteTeam(string userId, Guid matchId, bool useCache = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _teamService.GetUserFavTeam(userId, matchId, useCache, cancellationToken);
                return ReturnResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        [Route("GetTeams")]
        public async Task<ActionResult> GetTeams(bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return ReturnListResult(await _teamService.GetAllAsync(useCache, cancellationToken: cancellationToken));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTeam")]
        public async Task<ActionResult> GetTeamById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddTeam")]
        public async Task<ActionResult> AddTeam([FromBody] TeamDto team, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.CreateAsync(team, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> AddTeam([FromBody] CreateOrUpdateTeamDto team, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.CreateAsync(team, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("EditTeam")]
        public async Task<ActionResult> EditTeam([FromBody] TeamDto team, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.UpdateAsync(team, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPut]
        [Route("EditTeam/{id:guid}")]
        public async Task<ActionResult> EditTeam(Guid id, [FromBody] CreateOrUpdateTeamDto team, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.UpdateAsync(id, team, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("RecoverTeam")]
        public async Task<ActionResult> RecoverTeamById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.RecoverByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("SoftDeleteTeam")]
        public async Task<ActionResult> SoftDeleteTeamById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.SoftDeleteByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("HardDeleteTeam")]
        public async Task<ActionResult> HardDeleteTeamById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("DeleteTeamWithNoMatches/{id:guid}")]
        public async Task<ActionResult> DeleteTeamWithNoMatches(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _teamService.DeleteTeamWithNoMatchesAsync(id, cancellationToken: cancellationToken));
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
