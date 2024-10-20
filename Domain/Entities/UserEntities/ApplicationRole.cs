using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserEntities
{
    public class ApplicationRole : IdentityRole<long>
    {
        public string? Description { get; set; }

    }
}
