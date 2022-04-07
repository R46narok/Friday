using Friday.Application;
using Friday.Services.Authorization.Entities;
using Friday.Services.Authorization.Grpc.Services;
using Friday.Services.Authorization.Persistence;
using Friday.Services.Authorization.Services;
using Friday.Services.Authorization.Services.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddApplication(typeof(IIdentityService).Assembly);
builder.Services.AddTransient<IIdentityService, IdentityService>();

builder.Services.AddDbContext<AuthorizationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthorizationDbContext>();

var app = builder.Build();

app.MapGrpcService<UserService>();
app.Run();

