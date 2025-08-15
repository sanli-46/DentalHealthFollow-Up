using DentalHealthFollow_Up.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Microsoft.Maui.Storage;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class StatusPage : ContentPage
    {
        private IHttpClientFactory? _httpFactory;
        private readonly ObservableCollection<object> _last7Days = new();
        private string? _selectedImageBase64;

        private int CurrentUserId => Preferences.Get("CurrentUserId", 0);

        public StatusPage()
        {
            InitializeComponent();
            Last7DaysList.ItemsSource = _last7Days;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _httpFactory ??= ServiceHelper.Resolve<IHttpClientFactory>();

            if (CurrentUserId <= 0)
            {
                await DisplayAlert("Uyarı", "Lütfen giriş yapın.", "Tamam");
                await Navigation.PushAsync(new LoginPage());
                return;
            }

            await LoadGoals();
            await LoadLast7Days();
        }

        private async Task LoadGoals()
        {
            var client = _httpFactory!.CreateClient("API");
            var goals = await client.GetFromJsonAsync<List<GoalDto>>($"/api/Goals/user/{CurrentUserId}");
            GoalPicker.ItemsSource = goals ?? new List<GoalDto>();
        }

        private async Task LoadLast7Days()
        {
            _last7Days.Clear();
            var client = _httpFactory!.CreateClient("API");
            var records = await client.GetFromJsonAsync<List<GoalRecordDto>>($"/api/GoalRecords/last7days/{CurrentUserId}");
            if (records is null) return;

            foreach (var r in records)
            {
                _last7Days.Add(new
                {
                    Date = r.Date.ToString("dd.MM.yyyy"),
                    GoalTitle = $"Hedef #{r.GoalId}",
                    DurationText = r.DurationInMinutes.HasValue ? $"Süre: {r.DurationInMinutes} dk" : "Süre: -",
                    r.Note
                });
            }
        }

        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = "Görsel seç (.jpg/.png)" });
            if (result is null) return;

            using var stream = await result.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var bytes = ms.ToArray();

            _selectedImageBase64 = Convert.ToBase64String(bytes);
            SelectedImagePreview.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            SelectedImagePreview.IsVisible = true;
        }

        private async void OnSaveStatusClicked(object sender, EventArgs e)
        {
            if (GoalPicker.SelectedItem is not GoalDto selectedGoal)
            {
                await DisplayAlert("Uyarı", "Hedef seçiniz.", "Tamam");
                return;
            }

            int? duration = null;
            if (int.TryParse(DurationEntry.Text, out var d) && d > 0) duration = d;

            var dto = new GoalRecordCreateDto
            {
                UserId = CurrentUserId,
                GoalId = selectedGoal.Id,
                Date = DateTime.Today,
                DurationInMinutes = duration,
                Note = StatusNoteEditor.Text,
                ImageBase64 = _selectedImageBase64
            };

            var client = _httpFactory!.CreateClient("API");
            var resp = await client.PostAsJsonAsync("/api/GoalRecords", dto);

            if (resp.IsSuccessStatusCode)
            {
                await DisplayAlert("Başarılı", "Durum kaydedildi.", "Tamam");
                await LoadLast7Days();
            }
            else
            {
                var msg = await resp.Content.ReadAsStringAsync();
                await DisplayAlert("Hata", string.IsNullOrWhiteSpace(msg) ? "Kayıt başarısız." : msg, "Tamam");
            }
        }
    }
}
