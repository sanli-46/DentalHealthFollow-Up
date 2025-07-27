using DentalHealthFollow_Up;
using DentalHealthFollow_Up.DataAccess;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml.Controls;
using Windows.Networking.NetworkOperators;


namespace DentalHealthFollow_Up;

public partial class MainPage : ContentPage
{
    private readonly AppDbContext _context;
    public MainPage(AppDbContext context, string userEmail)
    {

        InitializeComponent();
        _context = context;
        welcomeLabel.Text = $"Hoş geldin, {userEmail}";
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
        await Navigation.PushAsync(new GoalsPage(_context));

    }

    private async void OnTipsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(TipsPage));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }
}

