using Vex.Components;
using Blazorise;
using Blazorise.Tailwind;
using Blazorise.Icons.FontAwesome;
using Vex.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("VEX_DB_CS", EnvironmentVariableTarget.Machine);

if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException("VEX_DB_CS environment variable is not set.");
}

// Add services to the container.
builder.Services
    .AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString))
    .AddBlazorise()
    .AddTailwindProviders()
    .AddFontAwesomeIcons()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
