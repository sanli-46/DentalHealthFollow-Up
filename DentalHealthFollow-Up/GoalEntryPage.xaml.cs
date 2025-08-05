using DentalHealthFollow_Up.Shared.DTOs;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI;

public partial class GoalEntryPage : ContentPage
{
    private readonly HttpClient _httpClient;
 public GoalEntryPage()
    {
        InitializeComponent();
    }
    public GoalEntryPage(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();
        _httpClient = httpClientFactory.CreateClient("API");
        LoadGoals(); // hedef listelemesi
    }
   


    private async void LoadGoals()
    {
        try
        {
            var goals = await _httpClient.GetFromJsonAsync<List<GoalDto>>("/api/goals?userId=1");
            GoalsCollection.ItemsSource = goals;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }

    private async void OnSaveGoalClicked(object sender, EventArgs e)
    {
        var dto = new GoalCreateDto
        {
            Title = TitleEntry.Text?.Trim(),
            Description = DescriptionEditor.Text?.Trim(),
            Period = PeriodPicker.SelectedItem?.ToString(),
            Importance = ImportancePicker.SelectedItem?.ToString(),
            UserId = 1
        };

        var response = await _httpClient.PostAsJsonAsync("/api/goals", dto);
        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Ba�ar�l�", "Hedef kaydedildi", "Tamam");
            LoadGoals(); // yeniden listele
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Kay�t ba�ar�s�z: {error}", "Tamam");
        }
    }

    private async void OnDeleteGoalClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int goalId)
        {
            var confirmed = await DisplayAlert("Sil", "Bu hedefi silmek istedi�inize emin misiniz?", "Evet", "Hay�r");
            if (!confirmed) return;

            var response = await _httpClient.DeleteAsync($"/api/goals/{goalId}");
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Ba�ar�l�", "Hedef silindi", "Tamam");
                LoadGoals();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Hata", $"Silme ba�ar�s�z: {error}", "Tamam");
            }
        }
    }
}

