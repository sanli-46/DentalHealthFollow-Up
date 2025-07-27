
using DentalHealthFollow_Up.Entities;
using DentalHealthFollow_Up.Helper;
using System.Net.Http.Json;


namespace DentalHealthFollow_Up;

public partial class RegisterPage : ContentPage
{

    public RegisterPage()
    {
        InitializeComponent();
     
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string fullName = fullNameEntry.Text;
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        string confirmPassword = confirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun.", "Tamam");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Hata", "Parolalar eþleþmiyor!", "Tamam");
            return;
        }

        var user = new
        {
            Name = fullName,
            Email = email,
            Password = password,
            BirthDate = DateTime.Now // Senin Entry'nden gelen tarih olabilir
        };

        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:7092/api/user/register", user);

        if (response.IsSuccessStatusCode)
        {
            await DisplayAlert("Baþarýlý", "Kayýt tamamlandý!", "Tamam");
            await Navigation.PushAsync(new LoginPage());
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Hata", $"Kayýt baþarýsýz: {error}", "Tamam");
        }
    }

}
