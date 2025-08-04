using System.Net.Http.Json;
using DentalHealthFollow_Up.Shared.DTOs;

namespace DentalHealthFollow_Up.MAUI;

public partial class ForgotPasswordPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private int userId = 0;

    public ForgotPasswordPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7146") }; // URL'yi API adresine g�re g�ncelle
    }

    private async void OnVerifyClicked(object sender, EventArgs e)
    {
        MessageLabel.IsVisible = false;
        NewPasswordLayout.IsVisible = false;

        var email = EmailEntry.Text?.Trim();
        if (string.IsNullOrEmpty(email))
        {
            MessageLabel.Text = "E-posta alan� bo� olamaz.";
            MessageLabel.IsVisible = true;
            return;
        }

        var response = await _httpClient.GetAsync($"/api/User/by-email/{email}");

        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            userId = user!.Id;
            NewPasswordLayout.IsVisible = true;
        }
        else
        {
            MessageLabel.Text = "E-posta adresi sistemde kay�tl� de�il.";
            MessageLabel.IsVisible = true;
        }
    }

    private async void OnUpdatePasswordClicked(object sender, EventArgs e)
    {
        MessageLabel.IsVisible = false;

        if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            MessageLabel.Text = "Parolalar e�le�miyor.";
            MessageLabel.IsVisible = true;
            return;
        }

        var update = new UserUpdateDto
        {
            Password = NewPasswordEntry.Text!
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/User/password-reset/{userId}", update);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Ba�ar�l�", "Parola g�ncellendi", "Tamam");
            await Shell.Current.GoToAsync("LoginPage");
        }
        else
        {
            MessageLabel.Text = "Parola g�ncellenemedi.";
            MessageLabel.IsVisible = true;
        }
    }
}
