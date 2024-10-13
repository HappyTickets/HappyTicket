using Application.Interfaces.IContactService;
using Domain.Entities.ContactEntity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactService _contactService;
        public ContactController(IHttpContextAccessor httpContextAccessor, IContactService contactService) : base(httpContextAccessor)
        {
            _contactService = contactService;
        }
        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] Contact contact, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _contactService.SendMessageAsync(contact, cancellationToken);
                    return Ok("Message Sent Successfully");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message} Please Try Again");
                }
            }

            return BadRequest("Invalid Data Submitted");
        }
    }
}
