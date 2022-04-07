using Friday.Domain.Events;
using Friday.Infrastructure.MessageBrokers.AzureServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

DomainEvents.RegisterHandlersFromAssembly(typeof(Program).Assembly, builder.Services);
AzureServiceBus.RegisterAzureServiceBusSubscriptionReceivers("Endpoint=sb://friday.servicebus.windows.net/;SharedAccessKeyName=ReadWrite;SharedAccessKey=CzlBOTRy3+5Z24lk+thurBeUFdJ5m4PKVZi7EOElPEw=;EntityPath=user",
    "user", "Cloud");
var app = builder.Build();
AzureServiceBus.Provider = app.Services;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();