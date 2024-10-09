using Application.Interfaces.Persistence;
using Domain.Entities.UserEntities;
using Infrastructure.Persistence.EntityFramework;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Authorization.Request;
using Shared.DTOs.Authorization.Response;

namespace Infrastructure.Persistence.Identity
{
    public class AuthorizationRepository(UserManager<ApplicationUser> userManager, AppDbContext context) : IAuthorizationRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Result<Unit>> UnassignUsersFromRoleAsync(RemoveUsersFromRoleDto dto, CancellationToken cancellationToken = default)
        {
            try
            {

                var userRoles = await _context.UserRoles
                    .Where(ur => ur.RoleId == dto.RoleId && dto.UserIds.Contains(ur.UserId))
                    .ToListAsync(cancellationToken);

                _context.UserRoles.RemoveRange(userRoles);

                await _context.SaveChangesAsync(cancellationToken);

                return new(Unit.Default); // Return success result
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<Unit>> AssignUsersToRoleAsync(AssignUsersToRoleDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var newUserRoles = new List<IdentityUserRole<string>>();

                foreach (var userId in dto.UserIds)
                {
                    var existingAssignment = await _context.UserRoles
                        .AnyAsync(ur => ur.RoleId == dto.RoleId && ur.UserId == userId, cancellationToken);

                    if (!existingAssignment)
                    {
                        newUserRoles.Add(new()
                        {
                            UserId = userId,
                            RoleId = dto.RoleId
                        });
                    }
                }

                // Bulk insert all new user-role mappings
                await _context.UserRoles.AddRangeAsync(newUserRoles, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new(Unit.Default); // Return success result
            }
            catch (Exception ex)
        {
                return new(ex); // Return failure result if something goes wrong
            }
        }

        public IQueryable<ApplicationUser> GetUsersInRole(string roleId)
        {
            var usersRoleQuery = _context.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Select(ur => ur.UserId);

            var usersInRoleQuery = _context.Users.Where(u => usersRoleQuery.Contains(u.Id));

            return usersInRoleQuery;
        }
        public IEnumerable<UserWithRolesDto> GetUsersWithRolesAsync(IQueryable<ApplicationUser> users, CancellationToken cancellationToken)
        {
            try
            {
                var query = from user in users
                            join userRole in _context.UserRoles on user.Id equals userRole.UserId into userRolesGroup
                            from userRole in userRolesGroup.DefaultIfEmpty() // Left join
                            join role in _context.Roles on userRole.RoleId equals role.Id into rolesGroup
                            from role in rolesGroup.DefaultIfEmpty() // Left join
                            group role by new { user.Id, user.UserName, user.Email } into userGroup
                            select new UserWithRolesDto
                            {
                                UserId = userGroup.Key.Id,
                                UserName = userGroup.Key.UserName,
                                Email = userGroup.Key.Email,
                                AssignedRoles = userGroup.Where(r => r != null).Select(r => r.Name).ToList() // Handle null roles
                            };

                return query.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
