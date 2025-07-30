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

            
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using var client = new HttpClient(handler);
            try
            {
               

                var response = await client.PostAsJsonAsync("https://192.168.238.1:7250/api/user/register", user);


                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Başarılı", "Kayıt tamamlandı!", "Tamam");
                    await Shell.Current.GoToAsync("//LoginPage");
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

