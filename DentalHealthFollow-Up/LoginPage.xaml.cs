using DentalHealthFallow_Up;
using DentalHealthFallow_Up.DataAccess;
using DentalHealthFallow_Up.Helpers;

namespace DentalHealthFollow_Up;

public partial class LoginPage : ContentPage
{
    private readonly AppDbContext _context;

    public LoginPage(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        string hashedPassword = PasswordHasher.Hash(password);

        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == hashedPassword);

        if (user != null)
        {
            await DisplayAlert("Baþarýlý", "Giriþ baþarýlý!", "Tamam");

            await Shell.Current.GoToAsync(nameof(MainPage));
            await Shell.Current.GoToAsync(nameof(LoginPage));
            await Shell.Current.GoToAsync(nameof(MainPage));
            await Shell.Current.GoToAsync(nameof(MainPage), new Dictionary<string, object>
            {
                ["userEmail"] = email
            });

        }
        else
        {
            await DisplayAlert("Hata", "Email veya þifre yanlýþ.", "Tamam");
        }
    }
}
