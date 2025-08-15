using Microsoft.Maui;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class TipsPage : ContentPage
    {
        public TipsPage() { InitializeComponent(); }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var tip = await Api.Client().GetStringAsync("api/HealthTips/random");
                tipLabel.Text = string.IsNullOrWhiteSpace(tip) ? "Hen�z �neri yok." : tip;
            }
            catch { tipLabel.Text = "�neri al�namad�."; }
        }
    }
}
