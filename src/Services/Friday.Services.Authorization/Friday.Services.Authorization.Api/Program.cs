using System.Reflection;
using Friday.Application;
using Friday.Domain.Events;
using Friday.Domain.Infrastructure.MessageBrokers;
using Friday.Infrastructure.MessageBrokers.AzureServiceBus;
using Friday.Services.Authorization.Entities;
using Friday.Services.Authorization.Persistence;
using Friday.Services.Authorization.Services;
using Friday.Services.Authorization.Services.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(typeof(IIdentityService).Assembly);
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddDbContext<AuthorizationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthorizationDbContext>();

DomainEvents.RegisterHandlersFromAssembly(typeof(Program).Assembly, builder.Services);
AzureServiceBus.RegisterAzureServiceBusSubscriptionReceivers("Endpoint=sb://friday.servicebus.windows.net/;SharedAccessKeyName=ReadWrite;SharedAccessKey=CzlBOTRy3+5Z24lk+thurBeUFdJ5m4PKVZi7EOElPEw=;EntityPath=user",
    "user", "Cloud");
var app = builder.Build();
AzureServiceBus.Provider = app.Services;

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