using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;


namespace DentalHealthFollow_Up.MAUI
{
    public partial class App : Application
    {
        
        
           public App(AppShell appShell)
        {
            InitializeComponent();
            MainPage = MainPage = appShell;
        }



    }
}

