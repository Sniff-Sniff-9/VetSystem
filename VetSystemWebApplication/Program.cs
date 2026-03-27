using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net;
using VetSystemWebApplication;
using VetSystemWebApplication.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7146/api/") });
builder.Services.AddLocalStorageServices();
builder.Services.AddMudServices();
builder.Services.AddScoped<AppointmentsService>();
builder.Services.AddScoped<ClientsService>();
builder.Services.AddScoped<PetsService>();

await builder.Build().RunAsync();

