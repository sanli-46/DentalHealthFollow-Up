using DentalHealthFollow_Up.Shared.DTOs;  
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI;

public partial class StatusPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private List<GoalDto> _goals = new();
    private string? _imageBase64 = null;

    public StatusPage(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();
        _httpClient = httpClientFactory.CreateClient("API");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        base.OnAppearing();
        await LoadGoals();
        await LoadRecords();

        try
        {
            var userId = Preferences.Get("UserId", 1);
            var response = await _httpClient.GetAsync($"/api/goals?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                _goals = await response.Content.ReadFromJsonAsync<List<GoalDto>>() ?? new();
                GoalPicker.ItemsSource = _goals;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Görsel Seç (.jpg, .png)",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            using var stream = await result.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            _imageBase64 = Convert.ToBase64String(ms.ToArray());

            
            stream.Position = 0;
            SelectedImagePreview.Source = ImageSource.FromStream(() => stream);
            SelectedImagePreview.IsVisible = true;

            await DisplayAlert("Baþarýlý", "Görsel seçildi", "Tamam");
        }
    }
    private async Task LoadRecords()
    {
        try
        {
            var userId = Preferences.Get("UserId", 1);
            var response = await _httpClient.GetAsync($"/api/goalrecords?userId={userId}&lastDays=7");

            if (response.IsSuccessStatusCode)
            {
                var records = await response.Content.ReadFromJsonAsync<List<GoalRecordListDto>>();
                RecordListView.ItemsSource = records;
            }
            else
            {
                await DisplayAlert("Hata", "Kayýtlar getirilemedi", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }


    private async void OnSaveRecordClicked(object sender, EventArgs e)
    {
        if (GoalPicker.SelectedItem is not GoalDto selectedGoal)
        {
            await DisplayAlert("Hata", "Lütfen bir hedef seçiniz.", "Tamam");
            return;
        }

        var dto = new GoalRecordDto
        {
            GoalId = selectedGoal.Id,
            Date = DateTime.Today,
            Time = DateTime.Now.TimeOfDay,
            Note = StatusNoteEditor.Text,
            DurationInMinutes = 0,
            ImageBase64 = _imageBase64,
            UserId = 1
        };

        var response = await _httpClient.PostAsJsonAsync("/api/goalrecords", dto);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Baþarýlý", "Durum kaydedildi", "Tamam");
            StatusNoteEditor.Text = "";
            _imageBase64 = null;
        }
        else
        {
            var msg = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Kayýt baþarýsýz: {msg}", "Tamam");
        }
    }
    private async Task LoadGoals()
    {
        try
        {
            var userId = Preferences.Get("UserId", 1);
            var response = await _httpClient.GetAsync($"/api/goals?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var goals = await response.Content.ReadFromJsonAsync<List<GoalDto>>() ?? new();
                GoalPicker.ItemsSource = goals;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }


}
