using DentalHealthFollow_Up.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DentalHealthFollow_Up.MAUI;


namespace DentalHealthFollow_Up;

public static class MauiProgram
{
    public static IServiceProvider _serviceProvider { get; private set; } = null!;

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

        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7250"); // API'nin çalıştığı port
        });

        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DisSagligiDb;Trusted_Connection=True;TrustServerCertificate=True;"));

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<GoalsPage>();
        builder.Services.AddTransient<TrackingPage>();
        builder.Services.AddTransient<HealthTipsPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ForgotPasswordPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        var mauiApp = builder.Build();
        _serviceProvider = mauiApp.Services;

        return mauiApp;
    }
}
