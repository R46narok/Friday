using Friday.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Friday.Services.Authorization.Entities;

public class ApplicationUser : IdentityUser, IEntity<string>
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
}