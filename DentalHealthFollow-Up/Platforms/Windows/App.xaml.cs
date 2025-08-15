using Microsoft.UI.Xaml;
using DentalHealthFollow_Up.MAUI;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace DentalHealthFollow_Up.WinUI
{
    
    public partial class App : MauiWinUIApplication
    {
       
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}
