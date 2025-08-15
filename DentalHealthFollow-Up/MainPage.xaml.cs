using System;
using System.Net.Http;
using Microsoft.Maui.Controls;

namespace DentalHealthFollow_Up.MAUI;

[QueryProperty(nameof(UserEmail), "email")]
public partial class MainPage : ContentPage
{
    private readonly HttpClient _http; 
    public string UserEmail { get; set; } = string.Empty;

    public MainPage(IHttpClientFactory httpFactory)
    {
        InitializeComponent();
        _http = httpFactory.CreateClient("API"); 
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        welcomeLabel.Text = $"Hoş geldin, {UserEmail}";
    }

    private async void OnProfileClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(ProfilePage));

    private async void OnTrackingClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(TrackingPage));

    private async void OnGoalsClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(GoalsPage)); 

    private async void OnTipsClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(TipsPage));

    private async void OnLogoutClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("//LoginPage");
}
