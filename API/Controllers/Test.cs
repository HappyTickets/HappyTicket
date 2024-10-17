using Application.Interfaces.Infrastructure.Persistence;
using Domain.Entities;
using Domain.Enums;
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
            var ticket = new Ticket
            {
                MatchTeamId = 1,
                Price = 49.99m,
                Notes = "Front row seat",
                BlockId = 1,
                SeatId = 1, 
                DisplayForSale = true,
                Location = "Stadium A",
                Class = "VIP",
                TicketStatus = TicketStatus.Active, 
                SeatNumber = 1,
                ExternalGate = "Gate 1",
                InternalGate = "A1",
                IsActive = true,
                SoftDeleteCount = 0,
                BaseEntityStatus = null,
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                ModifiedBy = 1,
                ModifiedDate = null
            };

            await ufw.Tickets.CreateAsync(ticket, 2000);
            return Ok();
        }
    }
}
