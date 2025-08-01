using DentalHealthFollow_Up.DataAccess;
using Microsoft.Maui.Controls;

namespace DentalHealthFollow_Up.MAUI;

[QueryProperty(nameof(UserEmail), "email")]
public partial class MainPage : ContentPage
{
    private readonly AppDbContext _context;

    public string UserEmail { get; set; } = string.Empty;

    public MainPage(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        welcomeLabel.Text = $"Hoş geldin, {UserEmail}";
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void OnTrackingClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TrackingPage));
    }

    private async void OnGoalsClicked(object sender, EventArgs e)
    {
        var client = MauiProgram._serviceProvider.GetService<HttpClient>();
        await Navigation.PushAsync(new GoalsPage(client));
    }

    private async void OnTipsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TipsPage));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}

