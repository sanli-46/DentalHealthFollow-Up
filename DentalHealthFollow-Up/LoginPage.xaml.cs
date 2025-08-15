using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;
using DentalHealthFollow_Up.Shared.DTOs;

namespace DentalHealthFollow_Up.MAUI; 
public partial class LoginPage : ContentPage
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserSession _session;

   
    public LoginPage() : this(
        MauiProgram.Services.GetRequiredService<IHttpClientFactory>(),
        MauiProgram.Services.GetRequiredService<UserSession>())
    { }

    
    public LoginPage(IHttpClientFactory httpClientFactory, UserSession session)
    {
        InitializeComponent();
        _httpClientFactory = httpClientFactory;
        _session = session;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text?.Trim();
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Uyarı", "E-posta ve şifre giriniz.", "Tamam");
            return;
        }

        var client = _httpClientFactory.CreateClient("API");
        var resp = await client.PostAsJsonAsync("api/user/login",
            new UserLoginDto { Email = email, Password = password });

        if (!resp.IsSuccessStatusCode)
        {
            var msg = await resp.Content.ReadAsStringAsync();
            await DisplayAlert("Giriş başarısız",
                string.IsNullOrWhiteSpace(msg) ? resp.StatusCode.ToString() : msg, "Tamam");
            return;
        }

        var user = await resp.Content.ReadFromJsonAsync<UserDto>();
        if (user is null || user.UserId <= 0)
        {
            await DisplayAlert("Hata", "Geçersiz kullanıcı bilgisi döndü.", "Tamam");
            return;
        }
        Preferences.Set("CurrentUserId", user.UserId);
        _session.CurrentUser = user;
        Application.Current.MainPage = new AppShell();
    }
    private async void OnGoRegisterTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//register");
    }
}
