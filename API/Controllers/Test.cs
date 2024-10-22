using Application.Interfaces.Persistence;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/testttttt")]
    public class Test : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IUnitOfWork ufw)
        {
            var repo = ufw.Repository<MatchTeam>();
                return Ok(await repo.ListAsync());
            //return Ok();
        }
    }
}
