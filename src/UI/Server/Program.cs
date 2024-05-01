using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Host.UseLamar(registry => { registry.IncludeRegistry<UiServiceRegistry>(); });

var connectionString = Environment.GetEnvironmentVariable("OpenTelemetry.ConnectionString");

var otelResource = ResourceBuilder.CreateDefault()
    .AddService("ChurchBulletin");


if (connectionString != null)
{
    builder.Logging.AddOpenTelemetry(options =>
    {
        options.IncludeScopes = true;
        options.AddAzureMonitorLogExporter(config => config.ConnectionString = connectionString);
        options.SetResourceBuilder(otelResource); ;
    });

    using var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddAzureMonitorMetricExporter(config => config.ConnectionString = connectionString)
        .SetResourceBuilder(otelResource)
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .Build();

    using var tracerProvider = Sdk.CreateTracerProviderBuilder()
        .AddAzureMonitorTraceExporter(config => config.ConnectionString = connectionString)
        .SetResourceBuilder(otelResource)
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .Build();

    new ApplicationInsightsLiveMetricsStartup(connectionString).Start();

}


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