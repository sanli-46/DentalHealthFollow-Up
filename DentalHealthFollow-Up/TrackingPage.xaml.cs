using DentalHealthFollow_Up.Shared.DTOs;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class TrackingPage : ContentPage
    {
        private int CurrentUserId => Preferences.Get("CurrentUserId", 0);

        public TrackingPage() { InitializeComponent(); }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CurrentUserId <= 0) { await DisplayAlert("Uyarý", "Giriþ yapýn.", "Tamam"); return; }

            var data = await Api.Client().GetFromJsonAsync<List<GoalRecordDto>>(
                $"api/GoalRecords/user/{CurrentUserId}");
            recordsList.ItemsSource = data ?? new();
        }
    }
}

