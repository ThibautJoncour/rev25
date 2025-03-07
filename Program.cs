using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Blazorise;
using Radzen;






var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazorise();

builder.Services.AddScoped<DialogService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();

builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<DialogService>();


builder.Services.AddAuthorizationCore();
// Enregistrement du provider d'authentification personnalisé
builder.Services.AddScoped<CustomAuthenticationStateProvider>();


builder.Services.AddAuthorizationCore();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
