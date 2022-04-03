using Friday.Services.Authorization.Entities;
using Microsoft.AspNetCore.Identity;

namespace Friday.Services.Authorization.Identity;

public interface IIdentityService
{
    public Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    public Task<ApplicationUser> FindByIdAsync(string id);
    public Task<ApplicationUser> FindByNameAsync(string name);
}