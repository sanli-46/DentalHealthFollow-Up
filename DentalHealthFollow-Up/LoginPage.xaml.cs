using DentalHealthFollow_Up;
using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Helper;
using System.Net.Http.Json;


namespace DentalHealthFollow_Up.MAUI;

public partial class LoginPage : ContentPage
{
    private readonly HttpClient _client;

    public LoginPage(HttpClient client)
    {
        InitializeComponent();
        _client = client;

    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");

    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
            return;
        }

        var userLogin = new
        {
            Email = email,
            Password = password
        };


        var response = await _client.PostAsJsonAsync("https://localhost:7092/api/user/login", userLogin);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Ba�ar�l�", "Giri� yap�ld�.", "Tamam");
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Giri� ba�ar�s�z: {error}", "Tamam");
        }
    }

}
