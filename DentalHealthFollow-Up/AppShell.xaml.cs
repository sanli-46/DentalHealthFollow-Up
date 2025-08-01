using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
    }
}
