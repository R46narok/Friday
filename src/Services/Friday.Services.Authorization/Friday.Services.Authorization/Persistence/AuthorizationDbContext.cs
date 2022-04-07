using Friday.Services.Authorization.Entities;
using Microsoft.EntityFrameworkCore;

namespace Friday.Services.Authorization.Persistence;

public class AuthorizationDbContext : DbContext
{
    public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
    
    public AuthorizationDbContext()
    {
        
    }

    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
    {
        
    }
}