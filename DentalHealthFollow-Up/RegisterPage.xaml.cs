using DentalHealthFollow_Up.Shared.DTOs;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI;

public partial class RegisterPage : ContentPage
{
    private readonly HttpClient _client;

    public RegisterPage(HttpClient client)
    {
        InitializeComponent();
        _client = client;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string fullName = fullNameEntry.Text;
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        string confirmPassword = confirmPasswordEntry.Text;
        DateTime birthDate = birthDatePicker.Date;

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Hata", "Parolalar eşleşmiyor!", "Tamam");
            return;
        }

        var userDto = new UserRegisterDto
        {
            Name = fullName,
            Email = email,
            Password = password,
            BirthDate = birthDate
        };

        try
        {
            var response = await _client.PostAsJsonAsync("https://localhost:7250/api/user/register", userDto);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Başarılı", "Kayıt tamamlandı!", "Tamam");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Hata", $"Kayıt başarısız: {error}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Bağlantı Hatası", $"API'ye erişilemedi:\n{ex.Message}", "Tamam");
        }
    }
}
