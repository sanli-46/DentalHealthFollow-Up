using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage() { InitializeComponent(); }

        private async void OnVerifyClicked(object sender, EventArgs e)
        {
            try
            {
                var email = EmailEntry.Text?.Trim();
                if (string.IsNullOrWhiteSpace(email))
                {
                    await DisplayAlert("Uyar�", "E-posta giriniz.", "Tamam"); return;
                }

                var resp = await Api.Client().PostAsJsonAsync("api/user/forgot-password", new { Email = email! });
                if (resp.IsSuccessStatusCode)
                    await DisplayAlert("Bilgi", "Hesap bulundu. Yeni �ifreyi belirleyebilirsiniz.", "Tamam");
                else
                    await DisplayAlert("Uyar�", "E-posta bulunamad�.", "Tamam");
            }
            catch (Exception ex) { await DisplayAlert("Hata", ex.Message, "Tamam"); }
        }

        private async void OnUpdatePasswordClicked(object sender, EventArgs e)
        {
            try
            {
                var email = EmailEntry.Text?.Trim();
                var newPass = NewPasswordEntry.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPass))
                {
                    await DisplayAlert("Uyar�", "E-posta ve yeni parola zorunludur.", "Tamam"); return;
                }

                var resp = await Api.Client().PostAsJsonAsync("api/user/reset-password",
                    new { Email = email!, NewPassword = newPass! });

                if (resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Ba�ar�l�", "Parola g�ncellendi. Giri� yapabilirsiniz.", "Tamam");
                    await Navigation.PopAsync();
                }
                else
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(err) ? "��lem ba�ar�s�z." : err, "Tamam");
                }
            }
            catch (Exception ex) { await DisplayAlert("Hata", ex.Message, "Tamam"); }
        }
    }
}

