using DentalHealthFollow_Up.Shared.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace DentalHealthFollow_Up.MAUI; 
public partial class LoginPage : ContentPage
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly UserSession _session;
    private bool _inited;

    public LoginPage()
    {
        InitializeComponent();
    }


    public LoginPage(IHttpClientFactory http, UserSession session) : this()
    {
        _httpClientFactory = http;
        _session = session;
        _inited = true;
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
