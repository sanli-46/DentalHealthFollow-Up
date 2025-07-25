using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = MauiProgram._serviceProvider.GetService<AppShell>();


        }
    }
}
