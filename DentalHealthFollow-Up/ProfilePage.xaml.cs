using System.Net.Http.Json;
using DentalHealthFollow_Up.Shared.DTOs;
using Newtonsoft.Json;

namespace DentalHealthFollow_Up.MAUI;

public partial class ProfilePage : ContentPage
{
    private readonly HttpClient _httpClient;

    public ProfilePage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7250"); 
        LoadUserData();
    }

    private async void LoadUserData()
    {
        try
        {
            // Burada local storage'dan userId alman gerekir. Örnek:
            var userId = Preferences.Get("UserId", 0);

            var response = await _httpClient.GetAsync($"/api/User/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserDto>();

                NameEntry.Text = user!.Name;
                EmailEntry.Text = user.Email;
                BirthDatePicker.Date = user.BirthDate;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        MessageLabel.IsVisible = false;

        if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            MessageLabel.Text = "Parolalar uyuþmuyor.";
            MessageLabel.IsVisible = true;
            return;
        }

        var updatedUser = new UserUpdateDto
        {
            Name = NameEntry.Text,
            Email = EmailEntry.Text,
            BirthDate = BirthDatePicker.Date,
            Password = NewPasswordEntry.Text
        };

        var userId = Preferences.Get("UserId", 0);
        var response = await _httpClient.PutAsJsonAsync($"/api/User/{userId}", updatedUser);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Baþarýlý", "Bilgiler güncellendi", "Tamam");
        }
        else
        {
            var msg = await response.Content.ReadAsStringAsync();
            MessageLabel.Text = $"Hata: {msg}";
            MessageLabel.IsVisible = true;
        }
    }
}

