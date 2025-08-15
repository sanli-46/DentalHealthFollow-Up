using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up.MAUI;

public partial class HealthTipsPage : ContentPage
{
    private readonly IHttpClientFactory _clientFactory;

    public HealthTipsPage() : this(
        MauiProgram.Services.GetRequiredService<IHttpClientFactory>())
    { }

    public HealthTipsPage(IHttpClientFactory clientFactory)
    {
        InitializeComponent();
        _clientFactory = clientFactory;
    }

    private async void OnGetTipClicked(object sender, EventArgs e)
    {
        try
        {
            var client = _clientFactory.CreateClient("API");
            var tip = await client.GetStringAsync("api/tips/random");
            TipLabel.Text = tip;
        }
        catch (Exception ex)
        {
            TipLabel.Text = $"Hata: {ex.Message}";
        }
    }
}
