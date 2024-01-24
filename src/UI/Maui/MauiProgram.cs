using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Palermo.BlazorMvc;
using Microsoft.Extensions.Configuration; // Add this line
using System.Reflection; // Add this line

namespace UI.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<IUiBus>(provider => new MvcBus(NullLogger<MvcBus>.Instance));

            
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("UI.Maui.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);

            var baseAddress = builder.Configuration.GetValue<string>("BaseAddress") 
                                 ?? (DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7174" : "https://localhost:7174");

            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            builder.Services.AddSingleton(sp => new HttpClient(handler.GetPlatformMessageHandler()) { BaseAddress = new Uri(baseAddress) });
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}