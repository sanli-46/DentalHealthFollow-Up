using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DentalHealthFollow_Up.MAUI
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            
            builder.Services.AddHttpClient("API", c =>
            {
                c.BaseAddress = new Uri(GetApiBase());
                c.DefaultRequestVersion = new Version(2, 0);
                c.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
#if ANDROID || IOS || WINDOWS || MACCATALYST
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
#endif
            });

          
            builder.Services.AddSingleton<UserSession>();

            
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<StatusPage>();
            builder.Services.AddTransient<GoalEntryPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<HealthTipsPage>();
            builder.Services.AddTransient<GoalsPage>();
            builder.Services.AddSingleton<AppShell>(); 

            var app = builder.Build();

            
            ServiceHelper.Initialize(app.Services);

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

