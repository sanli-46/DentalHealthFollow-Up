using System.Net.Http.Json;
using DentalHealthFollow_Up.MAUI.Models;


namespace DentalHealthFollow_Up.MAUI;


public partial class ForgotPasswordPage : ContentPage
{
    private readonly HttpClient _httpClient = new();

    public ForgotPasswordPage()
    {
        InitializeComponent();
        _httpClient.BaseAddress = new Uri("https://localhost:7146/api/user/");
    }

    private async void OnCheckEmailClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text?.Trim();
        if (string.IsNullOrEmpty(email))
        {
            ResultLabel.Text = "L�tfen e-mail giriniz.";
            return;
        }

        var response = await _httpClient.GetAsync($"forgot-password?email={email}");

        if (response.IsSuccessStatusCode)
        {
            ResultLabel.Text = "Email bulundu. Yeni �ifre giriniz.";
            ResetPanel.IsVisible = true;
        }
        else
        {
            ResultLabel.Text = "Email sistemde kay�tl� de�il.";
        }
    }

    private async void OnResetPasswordClicked(object sender, EventArgs e)
    {
        if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ResultLabel.Text = "�ifreler uyu�muyor!";
            return;
        }

        var dto = new PasswordResetDto
        {
            Email = EmailEntry.Text.Trim(),
            NewPassword = NewPasswordEntry.Text.Trim()
        };

        var response = await _httpClient.PostAsJsonAsync("reset-password", dto);

        if (response.IsSuccessStatusCode)
            ResultLabel.Text = "Parola ba�ar�yla g�ncellendi.";
        else
            ResultLabel.Text = "Hata: �ifre g�ncellenemedi.";
    }
}