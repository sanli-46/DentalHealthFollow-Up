using DentalHealthFallow_Up.DataAccess;
using DentalHealthFallow_Up.Entities;
using DentalHealthFallow_Up.Helpers;


namespace DentalHealthFollow_Up;

public partial class RegisterPage : ContentPage
{

  
    private readonly AppDbContext _context;

    public RegisterPage(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string fullName = fullNameEntry.Text;
        string email = emailEntry.Text;
        string password = passwordEntry.Text;
        string confirmPassword = confirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Hata", "�ifreler e�le�miyor.", "Tamam");
            return;
        }
       
        var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
        if (existingUser != null) {
            await DisplayAlert("Hata", "Bu eposta adresi zaten kayitli.", "Tamam");
            return;
        }

        var hashedPassword = PasswordHasher.Hash(password);

        var user = new User
        {
            Email = email,
            Password = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(); 

        // Ge�ici ba�ar� bildirimi
        await DisplayAlert("Ba�ar�l�", "Kay�t i�lemi tamamland�!", "Tamam");

        // TODO: Veritaban�na kay�t eklenecek (sonraki ad�m)
        await Navigation.PopAsync();
}
}
