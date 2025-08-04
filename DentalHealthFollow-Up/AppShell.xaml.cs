using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;


namespace DentalHealthFollow_Up.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(GoalsPage), typeof(GoalsPage));
            Routing.RegisterRoute(nameof(HealthTipsPage), typeof(HealthTipsPage));
            Routing.RegisterRoute(nameof(TrackingPage), typeof(TrackingPage));
            Routing.RegisterRoute(nameof(TipsPage), typeof(TipsPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

        }
    }
}
