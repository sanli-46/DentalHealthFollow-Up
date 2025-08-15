using DentalHealthFollow_Up.Shared.DTOs;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage() { InitializeComponent(); }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            try
            {
                var name = fullNameEntry.Text?.Trim();
                var email = emailEntry.Text?.Trim();
                var password = passwordEntry.Text;
                var birth = birthDatePicker.Date;

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    await DisplayAlert("Uyarı", "İsim, e-posta ve parola zorunludur.", "Tamam");
                    return;
                }

                var dto = new UserRegisterDto
                {
                    Name = name!,
                    Email = email!,
                    Password = password!,
                    BirthDate = birth
                };

                var resp = await Api.Client().PostAsJsonAsync("api/user/register", dto);
                if (!resp.IsSuccessStatusCode)
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(err) ? "Kayıt başarısız." : err, "Tamam");
                    return;
                }

                await DisplayAlert("Başarılı", "Hesap oluşturuldu. Giriş yapabilirsiniz.", "Tamam");
                await Navigation.PopAsync(); 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
}

