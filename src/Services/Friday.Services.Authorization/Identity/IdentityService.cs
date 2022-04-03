using Friday.Services.Authorization.Entities;
using Microsoft.AspNetCore.Identity;

namespace Friday.Services.Authorization.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
    {
        return _userManager.CreateAsync(user, password);
    }

    public Task<ApplicationUser> FindByIdAsync(string id)
    {
        return _userManager.FindByIdAsync(id);
    }

    public Task<ApplicationUser> FindByNameAsync(string name)
    {
        return _userManager.FindByNameAsync(name);
    }
}