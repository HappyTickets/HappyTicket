using Application.Interfaces.Persistence;
using Domain.Entities;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/testttttt")]
    public class Test : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] AppDbContext ufw)
        {
        //    var repo = ufw.Repository<Order>();
        //    return Ok(await repo.ListAsync());
            return Ok();
        }
    }
}
