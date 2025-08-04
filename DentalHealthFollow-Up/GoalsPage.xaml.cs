using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI;

public partial class GoalsPage : ContentPage
{
    private readonly HttpClient _client;

    public GoalsPage()
    {
        InitializeComponent();

        
        _client = MauiProgram._serviceProvider.GetRequiredService<HttpClient>();

      
    }

}
