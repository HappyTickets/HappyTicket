using Domain.Entities.UserEntities;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Authorization.Request;

namespace Infrastructure.Persistence.Identity
{
    public class AuthorizationRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task AssignRoleToUsers(AssignUsersToRoleDto assignUsersToRoleDto)
        {
            //var roles = new List<IdentityUserRole<string>>();
            var roles = assignUsersToRoleDto
                .UserIds
                .Select(id => new IdentityUserRole<string>
                { UserId = id, RoleId = assignUsersToRoleDto.Role });

            try
            {
                await _context.UserRoles.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("An error occurred while assigning roles.", ex);
            }
        }
    }
}
