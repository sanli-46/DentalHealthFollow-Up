using DentalHealthFollow_Up.Shared.DTOs;
using Microsoft.Maui.Storage;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class GoalEntryPage : ContentPage
    {
        private readonly IHttpClientFactory _httpFactory;
        private int CurrentUserId => Preferences.Get("CurrentUserId", 0);

        public GoalEntryPage() : this(ServiceHelper.Resolve<IHttpClientFactory>()) { }

        public GoalEntryPage(IHttpClientFactory httpFactory)
        {
            InitializeComponent();
            _httpFactory = httpFactory;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CurrentUserId <= 0)
            {
                await DisplayAlert("Uyarý", "Lütfen giriþ yapýn.", "Tamam");
                await Shell.Current.GoToAsync("//login");
                return;
            }
            await LoadGoals(); // sayfada liste varsa
        }

        private async Task LoadGoals()
        {
            try
            {
                var client = _httpFactory.CreateClient("API");
                var goals = await client.GetFromJsonAsync<List<GoalDto>>($"/api/Goals/user/{CurrentUserId}");
                GoalsCollectionView.ItemsSource = goals ?? new List<GoalDto>(); // XAML’de varsa
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }

        private async void OnSaveGoalClicked(object sender, EventArgs e)
        {
            if (CurrentUserId <= 0)
            {
                await DisplayAlert("Uyarý", "Giriþ gerekli.", "Tamam");
                return;
            }

            var dto = new GoalCreateDto
            {
                UserId = CurrentUserId,
                Title = TitleEntry.Text,
                Description = DescriptionEditor.Text,
                Period = PeriodPicker.SelectedItem?.ToString(),
                DurationInMinutes = int.TryParse(TargetDurationEntry.Text, out var t) ? t : null,
                Importance = ImportancePicker.SelectedItem?.ToString()
            };

            try
            {
                var client = _httpFactory.CreateClient("API");
                var resp = await client.PostAsJsonAsync("/api/Goals", dto);
                if (resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Baþarýlý", "Hedef kaydedildi.", "Tamam");
                    await LoadGoals();
                }
                else
                {
                    var msg = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(msg) ? "Kayýt baþarýsýz." : msg, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }

        private async void OnDeleteGoalClicked(object sender, EventArgs e)
        {
            if (sender is not Button btn || btn.BindingContext is not GoalDto g) return;

            var ok = await DisplayAlert("Sil", $"“{g.Title}” silinsin mi?", "Evet", "Hayýr");
            if (!ok) return;

            try
            {
                var client = _httpFactory.CreateClient("API");
                var resp = await client.DeleteAsync($"/api/Goals/{g.GoalId}");
                if (resp.IsSuccessStatusCode)
                    await LoadGoals();
                else
                    await DisplayAlert("Hata", await resp.Content.ReadAsStringAsync(), "Tamam");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }

        }
    }
}
