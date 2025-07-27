using DentalHealthFollow_Up.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DentalHealthFollow_Up;


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

        // ✅ DbContext tanımı
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=DisSagligiDb;Trusted_Connection=True;TrustServerCertificate=True;"));
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<AppShell>();


        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        var mauiApp = builder.Build();
        _serviceProvider = mauiApp.Services;
      
        
        return mauiApp;
    }

 public static IServiceProvider _serviceProvider { get; private set; }
}