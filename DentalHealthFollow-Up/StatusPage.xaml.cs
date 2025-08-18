using DentalHealthFollow_Up.Shared.DTOs;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class StatusPage : ContentPage
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ObservableCollection<object> _last7Days = new();
        private string? _selectedImageBase64;

        private int CurrentUserId => Preferences.Get("CurrentUserId", 0);

        // XAML için parametresiz ctor DI'ya delege etsin
        public StatusPage() : this(ServiceHelper.Resolve<IHttpClientFactory>()) { }

        // Asıl ctor
        public StatusPage(IHttpClientFactory httpFactory)
        {
            InitializeComponent();
            _httpFactory = httpFactory;
            RecordsCollectionView.ItemsSource = _last7Days;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CurrentUserId <= 0)
            {
                await DisplayAlert("Uyarı", "Lütfen önce giriş yapın.", "Tamam");
                await Shell.Current.GoToAsync("//login");
                return;
            }
            await LoadGoals();
            await LoadLast7Days();
        }

        private async Task LoadGoals()
        {
            try
            {
                var client = _httpFactory.CreateClient("API");
                var goals = await client.GetFromJsonAsync<List<GoalDto>>($"/api/Goals/user/{CurrentUserId}");
                GoalPicker.ItemsSource = goals ?? new List<GoalDto>();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Hedefler yüklenemedi: {ex.Message}", "Tamam");
            }
        }

        private async Task LoadLast7Days()
        {
            try
            {
                _last7Days.Clear();
                var client = _httpFactory.CreateClient("API");

               
                var goals = await client.GetFromJsonAsync<List<GoalDto>>($"/api/Goals/user/{CurrentUserId}");
                var goalMap = goals?.ToDictionary(g => g.GoalId, g => g.Title) ?? new Dictionary<int, string>();

                
                var records = await client.GetFromJsonAsync<List<GoalRecordDto>>($"/api/GoalRecords/user/{CurrentUserId}/last7");
                if (records == null) return;

             
                foreach (var r in records)
                {
                    var title = goalMap.TryGetValue(r.GoalId, out var t) ? t : $"Hedef #{r.GoalId}";
                    _last7Days.Add(new
                    {
                        Date = r.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
                        GoalTitle = title,
                        DurationText = r.DurationInMinutes.HasValue ? $"Süre: {r.DurationInMinutes} dk" : "Süre: -",
                        r.Note
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Kayıtlar yüklenemedi: {ex.Message}", "Tamam");
            }
        }


        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = "Görsel seç (.jpg/.png)" });
            if (result is null) return;

            using var stream = await result.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            _selectedImageBase64 = Convert.ToBase64String(ms.ToArray());

            SelectedImagePreview.IsVisible = true;
            SelectedImagePreview.Source = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
        }

        private async void OnSaveStatusClicked(object sender, EventArgs e)
        {
            if (CurrentUserId <= 0)
            {
                await DisplayAlert("Uyarı", "Giriş gerekli.", "Tamam");
                return;
            }

            if (GoalPicker.SelectedItem is not GoalDto selectedGoal)
            {
                await DisplayAlert("Uyarı", "Lütfen bir hedef seçin.", "Tamam");
                return;
            }

            int? duration = null;
            if (!string.IsNullOrWhiteSpace(DurationEntry.Text) && int.TryParse(DurationEntry.Text, out var d)) duration = d;

            var dto = new GoalRecordCreateDto
            {
                UserId = CurrentUserId,
                GoalId = selectedGoal.GoalId,
                DurationInMinutes = duration,
                Note = StatusNoteEditor.Text,
                ImageBase64 = _selectedImageBase64
            };

            try
            {
                var client = _httpFactory.CreateClient("API");
                var resp = await client.PostAsJsonAsync("/api/GoalRecords", dto);

                if (resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Başarılı", "Durum kaydedildi.", "Tamam");
                    StatusNoteEditor.Text = string.Empty;
                    DurationEntry.Text = string.Empty;
                    _selectedImageBase64 = null;
                    SelectedImagePreview.IsVisible = false;
                    await LoadLast7Days();
                }
                else
                {
                    var msg = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(msg) ? "Kayıt başarısız." : msg, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
}
