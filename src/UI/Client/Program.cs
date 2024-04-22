using System.Collections;
using BlazorApplicationInsights;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Palermo.BlazorMvc;
using UI.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var ConnectionString = Environment.GetEnvironmentVariable("OpenTelemetry.ConnectionString");

ConnectionString ??=
        "InstrumentationKey=62370908-c8ab-42cb-85ca-e08f56998971;IngestionEndpoint=https://southcentralus-3.in.applicationinsights.azure.com/;LiveEndpoint=https://southcentralus.livediagnostics.monitor.azure.com/;ApplicationId=38d77475-c6fa-47a0-9488-e9d6b88c6a7b";


builder.Services.AddBlazorApplicationInsights(x =>
{
    x.ConnectionString = ConnectionString;
});

builder.Services.AddScoped<IUiBus>(provider => new MvcBus(NullLogger<MvcBus>.Instance));
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });



await builder.Build().RunAsync();
