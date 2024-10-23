namespace Infrastructure.Persistence.Extensions
{
    using Domain.Entities.UserEntities;
    using Microsoft.AspNetCore.Identity;
    using Shared.Common.Enums;
    using System.Threading.Tasks;

    namespace Infrastructure.Persistence
    {
        public class DbSeeder(RoleManager<ApplicationRole> roleManager)
        {
            private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

            public async Task SeedAsync()
            {
                await SeedAdminRoleAsync();
            }

            private async Task SeedAdminRoleAsync()
            {
                var adminRole = await _roleManager.FindByNameAsync(AppRoles.Admin.ToString());

                if (adminRole is null)
                {
                    // Create the admin role if it does not exist
                    adminRole = new ApplicationRole
                    {
                        Name = AppRoles.Admin.ToString(),
                        Description = "Administrator role with full access."
                    };

                    await _roleManager.CreateAsync(adminRole);
                }
            }
        }
    }

}
