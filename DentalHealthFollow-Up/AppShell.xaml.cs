using Microsoft.Extensions.DependencyInjection;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class AppShell : Shell
    {
        private bool _didRedirect;

        // XAML AppShell parametresiz çağırır; servisleri buradan çöz.
        public AppShell() : this(
            AppServices.Provider.GetRequiredService<UserSession>())
        { }

        // Asıl ctor (testler için enjekte edilebilir)
        public AppShell(UserSession session)
        {
            InitializeComponent();

            // Rotalar
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("status", typeof(StatusPage));
            Routing.RegisterRoute("goals", typeof(GoalEntryPage));
            Routing.RegisterRoute("profile", typeof(ProfilePage));

            _session = session;
        }

        private readonly UserSession _session;

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Sadece ilk kez çalışsın
            if (_didRedirect) return;
            _didRedirect = true;

            // Oturum yoksa Login’e, varsa Main’e
            if (_session?.CurrentUser is null)
                await GoToAsync("///login");
            else
                await GoToAsync("///main");
        }
    }
}




