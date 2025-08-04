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

        // "API" ad�nda tan�mlanm�� HttpClient'� kullan
        _httpClient = httpClientFactory.CreateClient("API");
    }

    private async void OnSaveGoalClicked(object sender, EventArgs e)
    {
        // �rnek UserId (ger�ekte giri� yapan kullan�c�dan al�nmal�)
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
            await DisplayAlert("Ba�ar�l�", "Hedef kaydedildi", "Tamam");
            TitleEntry.Text = "";
            DescriptionEditor.Text = "";
            PeriodPicker.SelectedIndex = -1;
            ImportancePicker.SelectedIndex = -1;

            OnAppearing();
        }
        else
        {
            var msg = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Kay�t ba�ar�s�z: {msg}", "Tamam");
        }

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var userId = Preferences.Get("UserId", 1); // Giri� yapan kullan�c�ya g�re al�nmal�

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

        var confirm = await DisplayAlert("Emin misiniz?", "Bu hedefi silmek istiyor musunuz?", "Evet", "Hay�r");
        if (!confirm)
            return;

        var response = await _httpClient.DeleteAsync($"/api/goals/{goalId}");

        if (response.IsSuccessStatusCode)
        {
            _goals.RemoveAll(g => g.Id == goalId);
            GoalsCollection.ItemsSource = null;
            GoalsCollection.ItemsSource = _goals;

            await DisplayAlert("Silindi", "Hedef ba�ar�yla silindi.", "Tamam");
        }
        else
        {
            await DisplayAlert("Hata", "Silme i�lemi ba�ar�s�z oldu.", "Tamam");
        }
    }

}

