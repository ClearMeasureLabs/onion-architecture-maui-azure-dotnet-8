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
           // builder.Services.AddScoped<IUiBus>(provider => new MvcBus(NullLogger<MvcBus>.Instance));

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}