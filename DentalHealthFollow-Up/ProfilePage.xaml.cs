using DentalHealthFollow_Up.Shared.DTOs;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class ProfilePage : ContentPage
    {
        private int CurrentUserId => Preferences.Get("CurrentUserId", 0);

        private readonly IHttpClientFactory _httpFactory;

       
        public ProfilePage() : this(ServiceHelper.Resolve<IHttpClientFactory>()) { }
        public ProfilePage(IHttpClientFactory httpFactory)
        {
            InitializeComponent();
            _httpFactory = httpFactory;
        }
       



        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CurrentUserId <= 0) { await DisplayAlert("Uyarý", "Giriþ yapýn.", "Tamam"); return; }

            var user = await Api.Client().GetFromJsonAsync<UserDto>($"api/user/{CurrentUserId}");
            if (user is null) return;

            NameEntry.Text = user.Name;
            EmailEntry.Text = user.Email;
            BirthDatePicker.Date = user.BirthDate == default ? DateTime.Today : user.BirthDate;
        }

        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            try
            {
                var dto = new UserUpdateDto
                {
                    
                    Name = NameEntry.Text?.Trim(),
                    Email = EmailEntry.Text?.Trim(),
                    BirthDate = BirthDatePicker.Date
                };

                var userId = Preferences.Get("CurrentUserId", 0);
                if (userId <= 0)
                {
                    await DisplayAlert("Uyarý", "Önce giriþ yapýn.", "Tamam");
                    return;
                }

                var resp = await Api.Client().PutAsJsonAsync($"api/user/{userId}", dto);
                if (resp.IsSuccessStatusCode)
                {
                    Preferences.Set("CurrentUserName", dto.Name ?? "");
                    Preferences.Set("CurrentUserEmail", dto.Email ?? "");
                    await DisplayAlert("Baþarýlý", "Profil güncellendi.", "Tamam");
                }
                else
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(err) ? "Güncelleme baþarýsýz." : err, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
}
