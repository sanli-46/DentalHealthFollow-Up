using DentalHealthFallow_Up.DataAccess;

namespace DentalHealthFollow_Up;

public partial class LoginPage : ContentPage
{
    private readonly AppDbContext _context;

    public LoginPage(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

        if (user != null)
        {
            await DisplayAlert("Baþarýlý", "Giriþ baþarýlý!", "Tamam");
            await Shell.Current.GoToAsync(nameof(MainPage));
            await Shell.Current.GoToAsync(nameof(LoginPage));
            await Shell.Current.GoToAsync(nameof(MainPage));

        }
        else
        {
            await DisplayAlert("Hata", "Email veya þifre yanlýþ.", "Tamam");
        }
    }
}
