using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserEntities.AuthEntities
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
