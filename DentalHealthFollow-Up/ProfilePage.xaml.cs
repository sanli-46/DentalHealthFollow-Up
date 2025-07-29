using DentalHealthFollow_Up.Entities;
using DentalHealthFollow_Up.DataAccess;


namespace DentalHealthFollow_Up.MAUI;

public partial class ProfilePage : ContentPage
{
    private readonly AppDbContext _context;
    private readonly string _email;

    public ProfilePage(AppDbContext context, string email)
    {
        InitializeComponent();
        _context = context;
        _email = email;

        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == _email);
        if (user != null)
        {
            fullNameEntry.Text = user.Name;
        }
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == _email);
        if (user == null)
        {
            await DisplayAlert("Hata", "Kullanýcý bulunamadý.", "Tamam");
            return;
        }

        string fullName = fullNameEntry.Text?.Trim() ?? "";
        string password = passwordEntry.Text?.Trim() ?? "";
        string confirm = confirmPasswordEntry.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(fullName))
        {
            await DisplayAlert("Hata", "Ad Soyad boþ olamaz.", "Tamam");
            return;
        }

        if (!string.IsNullOrEmpty(password))
        {
            if (password != confirm)
            {
                await DisplayAlert("Hata", "Þifreler uyuþmuyor.", "Tamam");
                return;
            }

            user.Password = password;
        }

        user.Name = fullName;
        await _context.SaveChangesAsync();

        await DisplayAlert("Baþarýlý", "Bilgiler güncellendi.", "Tamam");
    }
}
