using System.Net.Http.Json;
using DentalHealthFollow_Up.Shared.DTOs;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class MainPage : ContentPage
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly UserSession _session;

        public MainPage() : this(
           ServiceHelper.Resolve<IHttpClientFactory>(),
           ServiceHelper.Resolve<UserSession>()
            )
        { }
        public MainPage(IHttpClientFactory httpFactory, UserSession session)
        {
            InitializeComponent();
            _httpFactory = httpFactory;
            _session = session;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var name = _session.CurrentUser?.Name;
            WelcomeLabel.Text = string.IsNullOrWhiteSpace(name) ? "Hoş geldin!" : $"Hoş geldin, {name}!";

            await LoadTipAsync();
            await LoadLast7DaysAsync();
        }

        private HttpClient CreateApiClient()
            => _httpFactory.CreateClient("API");

       
        private async Task LoadTipAsync()
        {
            try
            {
                var client = CreateApiClient();
                
                var tipString = await client.GetStringAsync("/api/tips/random");
                TipLabel.Text = string.IsNullOrWhiteSpace(tipString)
                    ? "Diş fırçalama sonrası dili nazikçe temizlemeyi unutma."
                    : tipString;
            }
            catch
            {
                TipLabel.Text = "Şekerli gıdalardan sonra ağzı suyla çalkalamak çürük riskini azaltır.";
            }
        }

        private async Task LoadLast7DaysAsync()
        {
            try
            {
                var userId = _session.CurrentUser?.UserId;
                if (userId is null)
                {
                    SummaryList.ItemsSource = Array.Empty<SummaryVm>();
                    return;
                }

                var client = CreateApiClient();
                var records = await client.GetFromJsonAsync<List<GoalRecordDto>>($"/api/goalrecords/last7days/{userId}")
                              ?? new List<GoalRecordDto>();

                var list = records
                    .OrderByDescending(r => r.Date)
                    .Select(r => new SummaryVm
                    {
                        TitleDisplay = $"Hedef #{r.GoalId}",
                        DateDisplay = $"Tarih: {r.Date:dd.MM.yyyy HH:mm}",
                        DurationDisplay = r.DurationInMinutes.HasValue ? $"Süre (dk): {r.DurationInMinutes}" : "Süre (dk): -",
                        NoteDisplay = string.IsNullOrWhiteSpace(r.Note) ? "Not: -" : $"Not: {r.Note}"
                    })
                    .ToList();

                SummaryList.ItemsSource = list;
            }
            catch
            {
                SummaryList.ItemsSource = Array.Empty<SummaryVm>();
            }
        }

       
        private async void OnGoStatus(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//status");

        private async void OnGoGoals(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//goals");

        private async void OnGoProfile(object sender, EventArgs e)
            => await Shell.Current.GoToAsync("//profile");

        private async void OnRefreshTip(object sender, EventArgs e)
            => await LoadTipAsync();

        private async void OnLogout(object sender, EventArgs e)
        {
            _session.CurrentUser = null; // Shared/UserSession
            await Shell.Current.GoToAsync("///login");
        }

      
        private class SummaryVm
        {
            public string TitleDisplay { get; set; } = "";
            public string DateDisplay { get; set; } = "";
            public string DurationDisplay { get; set; } = "";
            public string NoteDisplay { get; set; } = "";
        }
    }
}




