using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;


namespace DentalHealthFollow_Up;

public partial class HealthTipsPage : ContentPage
{
    private readonly AppDbContext _context;
    private readonly Random _random = new();

    public HealthTipsPage(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
        ShowRandomTip();
    }

    private void ShowRandomTip()
    {
        var tips = _context.HealthTips.ToList();
        if (tips.Any())
        {
            var randomTip = tips[_random.Next(tips.Count)];
            tipLabel.Text = randomTip.Content;
        }
        else
        {
            tipLabel.Text = "Hen�z kay�tl� bir sa�l�k ipucu yok.";
        }
    }

    private void OnShowTipClicked(object sender, EventArgs e)
    {
        ShowRandomTip();
    }
}
