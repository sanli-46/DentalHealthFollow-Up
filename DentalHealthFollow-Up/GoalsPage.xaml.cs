using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;

namespace DentalHealthFollow_Up;

public partial class GoalsPage : ContentPage
{
    private readonly AppDbContext _context;

    public GoalsPage(AppDbContext context)
    {
      
        InitializeComponent();
       
        _context = context;
        LoadGoals();
    }
    private void LoadGoals()
    {
        var goals = _context.Goals.ToList();
        goalsCollectionView.ItemsSource = goals;
    }

    private async void OnAddGoalClicked(object sender, EventArgs e)
    {
        string title = newGoalEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlert("Hata", "Hedef boþ olamaz.", "Tamam");
            return;
        }

        var newGoal = new Goal { Title = title };
        _context.Goals.Add(newGoal);
        await _context.SaveChangesAsync();

        newGoalEntry.Text = "";
        LoadGoals();
    }
}
