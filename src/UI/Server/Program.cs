using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Host.UseLamar(registry => { registry.IncludeRegistry<UiServiceRegistry>(); });

const string serviceName = "ChurchBulletin";
const string conString =
    "InstrumentationKey=62370908-c8ab-42cb-85ca-e08f56998971;IngestionEndpoint=https://southcentralus-3.in.applicationinsights.azure.com/;LiveEndpoint=https://southcentralus.livediagnostics.monitor.azure.com/;ApplicationId=38d77475-c6fa-47a0-9488-e9d6b88c6a7b";

builder.Logging.AddOpenTelemetry(options =>
{
    options.AddAzureMonitorLogExporter(configo => configo.ConnectionString = conString);
    options.SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName));
});

var app = builder.Build();
//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHealthChecks("_healthcheck");

await app.Services.GetRequiredService<HealthCheckService>().CheckHealthAsync();

app.Run();