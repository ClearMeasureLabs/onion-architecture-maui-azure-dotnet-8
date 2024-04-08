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
// Create a new OpenTelemetry tracer provider.
// It is important to keep the TracerProvider instance active throughout the process lifetime.
var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddAzureMonitorTraceExporter(options =>
    {
        options.ConnectionString = conString;
    });

// Create a new OpenTelemetry meter provider.
// It is important to keep the MetricsProvider instance active throughout the process lifetime.
var metricsProvider = Sdk.CreateMeterProviderBuilder()
    .AddAzureMonitorMetricExporter(options =>
    {
        options.ConnectionString = conString;
    });

// Create a new logger factory.
// It is important to keep the LoggerFactory instance active throughout the process lifetime.
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(options =>
    {
        options.AddAzureMonitorLogExporter(options =>
        {
            options.ConnectionString = conString;
        }).SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService(serviceName));
    });
});

builder.Services.AddOpenTelemetry().ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing().WithMetrics();

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