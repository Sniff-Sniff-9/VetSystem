using MudBlazor.Services;
using System.Net;
using VetSystemModels.Dto;
using VetSystemWebApp.Components;
using VetSystemWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalStorageServices();
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp =>
{
    var handler = new HttpClientHandler
    {
        UseCookies = true,            // обязательно
        CookieContainer = new CookieContainer()
    };

    return new HttpClient(handler)
    {
        BaseAddress = new Uri("https://localhost:7146/api/")
    };
});
builder.Services.AddScoped<PetsService>();
builder.Services.AddScoped<AppointmentsService>();


builder.Services.AddMudServices();

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
