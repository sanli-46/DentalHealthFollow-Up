using DentalHealthFollow_Up;
using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Helper;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up;

public partial class LoginPage : ContentPage
{
    

    public LoginPage()
    {
        InitializeComponent();
        
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
            await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun.", "Tamam");
            return;
        }

        var userLogin = new
        {
            Email = email,
            Password = password
        };

        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:7092/api/user/login", userLogin);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Baþarýlý", "Giriþ yapýldý.", "Tamam");
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Giriþ baþarýsýz: {error}", "Tamam");
        }
    }

}
