using Vex.Components;
using Blazorise;
using Blazorise.Tailwind;
using Blazorise.Icons.FontAwesome;
using Vex.Data;
using Microsoft.EntityFrameworkCore;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("VEX_DB_CS", EnvironmentVariableTarget.Machine);

if (string.IsNullOrEmpty(connectionString))
{
    throw new ArgumentNullException("VEX_DB_CS environment variable is not set.");
}

var Auth0Domain = Environment.GetEnvironmentVariable("VEX_AUTH0_DOMAIN", EnvironmentVariableTarget.Machine);

if (string.IsNullOrEmpty(Auth0Domain))
{
    throw new ArgumentNullException("VEX_AUTH0_DOMAIN environment variable is not set.");
}

var Auth0ClientId = Environment.GetEnvironmentVariable("VEX_AUTH0_CLIENT_ID", EnvironmentVariableTarget.Machine);

if (string.IsNullOrEmpty(Auth0ClientId))
{
    throw new ArgumentNullException("VEX_AUTH0_CLIENT_ID environment variable is not set.");
}

// Add services to the container.
builder.Services
    .AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString))
    .AddBlazorise()
    .AddTailwindProviders()
    .AddFontAwesomeIcons()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = Auth0Domain;
    options.ClientId = Auth0ClientId;
});

builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/Account/Login", async (HttpContext httpContext, string returnUrl = "/") =>
{
    var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
        .WithRedirectUri(returnUrl)
        .Build();

    await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
});

app.MapGet("/Account/Logout", async (HttpContext httpContext) =>
{
    var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
        .WithRedirectUri("/")
        .Build();

    await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
});

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();