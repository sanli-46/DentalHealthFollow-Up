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
                    await DisplayAlert("Uyarý", "E-posta giriniz.", "Tamam"); return;
                }

                var resp = await Api.Client().PostAsJsonAsync("api/user/forgot-password", new { Email = email! });
                if (resp.IsSuccessStatusCode)
                    await DisplayAlert("Bilgi", "Hesap bulundu. Yeni þifreyi belirleyebilirsiniz.", "Tamam");
                else
                    await DisplayAlert("Uyarý", "E-posta bulunamadý.", "Tamam");
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
                    await DisplayAlert("Uyarý", "E-posta ve yeni parola zorunludur.", "Tamam"); return;
                }

                var resp = await Api.Client().PostAsJsonAsync("api/user/reset-password",
                    new { Email = email!, NewPassword = newPass! });

                if (resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Baþarýlý", "Parola güncellendi. Giriþ yapabilirsiniz.", "Tamam");
                    await Navigation.PopAsync();
                }
                else
                {
                    var err = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(err) ? "Ýþlem baþarýsýz." : err, "Tamam");
                }
            }
            catch (Exception ex) { await DisplayAlert("Hata", ex.Message, "Tamam"); }
        }
    }
}

