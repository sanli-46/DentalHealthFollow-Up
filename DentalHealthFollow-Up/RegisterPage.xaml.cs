using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace DentalHealthFollow_Up
{
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
            DateTime birthDate = birthDatePicker.Date;

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Hata", "Lütfen tüm alanları doldurun.", "Tamam");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Hata", "Parolalar eşleşmiyor!", "Tamam");
                return;
            }

            var user = new
            {
                Name = fullName,
                Email = email,
                Password = password,
                BirthDate = birthDate
            };

            using var client = new HttpClient();

            try
            {
                // 🔁 BURAYA kendi port numaranı yaz!
                var response = await client.PostAsJsonAsync("https://localhost:7146/api/user/register", user);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Başarılı", "Kayıt tamamlandı!", "Tamam");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", $"Kayıt başarısız: {error}", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Sunucuya bağlanılamadı: {ex.Message}", "Tamam");
            }
        }
    }
}

