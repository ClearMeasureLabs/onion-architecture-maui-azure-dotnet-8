using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;

namespace ProgrammingWithPalermo.ChurchBulletin.UI.Server;

public class ApplicationInsightsLiveMetricsStartup(string connectionString)
{
    public void Start()
    {
        // Create a TelemetryConfiguration instance.
        TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();
        config.ConnectionString = connectionString;
        QuickPulseTelemetryProcessor? quickPulseProcessor = null;
        config.DefaultTelemetrySink.TelemetryProcessorChainBuilder
            .Use((next) =>
            {
                quickPulseProcessor = new QuickPulseTelemetryProcessor(next);
                return quickPulseProcessor;
            })
            .Build();

        var quickPulseModule = new QuickPulseTelemetryModule();

        quickPulseModule.Initialize(config);
        quickPulseModule.RegisterTelemetryProcessor(quickPulseProcessor);

        // Create a TelemetryClient instance. It is important
        // to use the same TelemetryConfiguration here as the one
        // used to set up Live Metrics.
        TelemetryClient client = new TelemetryClient(config);

        // This sample runs indefinitely. Replace with actual application logic.
        
            // Send dependency and request telemetry.
            // These will be shown in Live Metrics.
            // CPU/Memory Performance counter is also shown
            // automatically without any additional steps.
            client.TrackDependency("My dependency", "target", "http://microsoft.com",
                DateTimeOffset.Now, TimeSpan.FromMilliseconds(300), true);
            client.TrackRequest("My Request", DateTimeOffset.Now,
                TimeSpan.FromMilliseconds(230), "200", true);
            Task.Delay(1000).Wait();
        
    }
}