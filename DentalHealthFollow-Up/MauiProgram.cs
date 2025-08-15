using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Logging;

namespace DentalHealthFollow_Up.MAUI
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; } = default!;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<UserSession>();

            builder.Services.AddHttpClient("API", c =>
            {
                c.BaseAddress = new Uri(GetApiBase());
                c.DefaultRequestVersion = new Version(2, 0);
                c.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            })
#if DEBUG
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            })
#endif
            ;
            builder.Services.AddSingleton<UserSession>();   // Shared içindeki UserSession'ı kullanıyoruz

            // Sayfalar (Shell ve içerikleri)
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<StatusPage>();
            builder.Services.AddTransient<GoalEntryPage>();
            builder.Services.AddTransient<ProfilePage>();

            builder.Services.AddSingleton<AppShell>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            var app = builder.Build();
            Services = app.Services;
            return app;
        }

        private static string GetApiBase()
        {
#if ANDROID
            return "https://10.0.2.2:7250/";
#elif IOS || MACCATALYST
            return "https://localhost:7250/";
#elif WINDOWS
            return "https://localhost:7250/";
#else
            return "https://localhost:7250/";
#endif
        }
    }
}

