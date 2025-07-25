using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up
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
