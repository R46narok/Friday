using Friday.Application;
using Friday.Services.Authorization.Entities;
using Friday.Services.Authorization.Identity;
using Friday.Services.Authorization.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(typeof(Program).Assembly);
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddDbContext<AuthorizationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Db"));
    });
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthorizationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<AuthorizationDbContext>();
    context!.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();