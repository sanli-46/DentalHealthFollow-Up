using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI;

public partial class GoalsPage : ContentPage
{
    private readonly HttpClient _client;

    public GoalsPage(HttpClient client)
    {
        InitializeComponent();
        _client = client;

        // Picker'larý doldur
        goalImportancePicker.ItemsSource = new List<string> { "Düþük", "Orta", "Yüksek" };
        goalPeriodPicker.ItemsSource = new List<string> { "Günde 1", "Haftada 1", "Ayda 1" };
    }

    private async void OnSaveGoalClicked(object sender, EventArgs e)
    {
        var goal = new
        {
            Title = goalTitleEntry.Text,
            Description = goalDescriptionEntry.Text,
            Period = goalPeriodPicker.SelectedItem?.ToString(),
            Importance = goalImportancePicker.SelectedItem?.ToString()
        };

        var response = await _client.PostAsJsonAsync("https://localhost:7092/api/goal", goal);

        if (response.IsSuccessStatusCode)
            await DisplayAlert("Baþarýlý", "Hedef kaydedildi", "Tamam");
        else
            await DisplayAlert("Hata", "Hedef kaydedilemedi", "Tamam");
    }

    private async void OnSaveNoteClicked(object sender, EventArgs e)
    {
        string note = noteEditor.Text;
        await DisplayAlert("Not Kaydedildi", note, "Tamam");
    }

    private async void OnDeleteGoalClicked(object sender, EventArgs e)
    {
        // Geçici silme örneði
        await DisplayAlert("Sil", "Silme iþlemi baþarýyla tamamlandý.", "Tamam");
    }
}


