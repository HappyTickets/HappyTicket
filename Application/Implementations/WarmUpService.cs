using Domain.Entities.UserEntities;
using Microsoft.AspNetCore.Identity;

namespace Application.Implementations;

public class WarmUpService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public WarmUpService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task WarmUpAsync()
    {
        await _userManager.FindByEmailAsync("Anas@gmail.com");
    }
}
