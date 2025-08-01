using DentalHealthFollow_Up.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DentalHealthFollow_Up.MAUI;
using CommunityToolkit.Maui;

namespace DentalHealthFollow_Up;

public static class MauiProgram
{
    public static IServiceProvider _serviceProvider { get; private set; } = null!;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddHttpClient();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DisSagligiDb;Trusted_Connection=True;TrustServerCertificate=True;"));

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<GoalsPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        var mauiApp = builder.Build();
        _serviceProvider = mauiApp.Services;

        return mauiApp;
    }
}
