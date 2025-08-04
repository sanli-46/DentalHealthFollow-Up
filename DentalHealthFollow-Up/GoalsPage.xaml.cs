using DentalHealthFollow_Up.Shared.DTOs;
using System.Net.Http.Json;
using DentalHealthFollow_Up.Shared.DTOs;

using static System.Net.Mime.MediaTypeNames;

namespace DentalHealthFollow_Up.MAUI;

public partial class GoalsPage : TabbedPage
{
    private readonly HttpClient _httpClient;
    private List<GoalDto> _goals = new();
    public GoalsPage(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();

        // "API" adýnda tanýmlanmýþ HttpClient'ý kullan
        _httpClient = httpClientFactory.CreateClient("API");
    }

    private async void OnSaveGoalClicked(object sender, EventArgs e)
    {
        // Örnek UserId (gerçekte giriþ yapan kullanýcýdan alýnmalý)
        var userId = Preferences.Get("UserId", 1);

        var dto = new GoalDto
        {
            UserId = userId,
            Title = TitleEntry.Text ?? "",
            Description = DescriptionEditor.Text ?? "",
            Period = PeriodPicker.SelectedItem?.ToString() ?? "",
            Importance = ImportancePicker.SelectedItem?.ToString() ?? ""
        };

        var response = await _httpClient.PostAsJsonAsync("/api/goals", dto);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Baþarýlý", "Hedef kaydedildi", "Tamam");
            TitleEntry.Text = "";
            DescriptionEditor.Text = "";
            PeriodPicker.SelectedIndex = -1;
            ImportancePicker.SelectedIndex = -1;

            OnAppearing();
        }
        else
        {
            var msg = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Kayýt baþarýsýz: {msg}", "Tamam");
        }

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var userId = Preferences.Get("UserId", 1); // Giriþ yapan kullanýcýya göre alýnmalý

        var response = await _httpClient.GetAsync($"/api/goals?userId={userId}");

        if (response.IsSuccessStatusCode)
        {
            _goals = await response.Content.ReadFromJsonAsync<List<GoalDto>>() ?? new();
            GoalsCollection.ItemsSource = _goals;
        }
    }
    private async void OnDeleteGoalClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var goalId = (int)button.CommandParameter;

        var confirm = await DisplayAlert("Emin misiniz?", "Bu hedefi silmek istiyor musunuz?", "Evet", "Hayýr");
        if (!confirm)
            return;

        var response = await _httpClient.DeleteAsync($"/api/goals/{goalId}");

        if (response.IsSuccessStatusCode)
        {
            _goals.RemoveAll(g => g.Id == goalId);
            GoalsCollection.ItemsSource = null;
            GoalsCollection.ItemsSource = _goals;

            await DisplayAlert("Silindi", "Hedef baþarýyla silindi.", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Silme iþlemi baþarýsýz oldu.", "Tamam");
        }
    }

}

