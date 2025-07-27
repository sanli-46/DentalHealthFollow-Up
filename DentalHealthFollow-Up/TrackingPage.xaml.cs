using DentalHealthFollow_Up.DataAccess;
using DentalHealthFollow_Up.Entities;
using Microsoft.Maui.Controls;

namespace DentalHealthFollow_Up;

public partial class TrackingPage : ContentPage
{
    private readonly AppDbContext _context;

    public TrackingPage(AppDbContext context)
    {
        InitializeComponent();
        _context = context;
        LoadTrackings();
    }

    private void LoadTrackings()
    {
        var records = _context.GoalRecords.ToList();  // Bu tablo hedef takibi için kullanýlýyor
        trackingCollectionView.ItemsSource = records;
    }

    private async void OnSaveTrackingClicked(object sender, EventArgs e)
    {
        string note = trackingNoteEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(note))
        {
            await DisplayAlert("Hata", "Boþ kayýt eklenemez.", "Tamam");
            return;
        }

        var record = new GoalRecord
        {
            Note = note,
            CreatedAt = DateTime.Now
        };

        _context.GoalRecords.Add(record);
        await _context.SaveChangesAsync();

        trackingNoteEntry.Text = "";
        LoadTrackings();
    }
}
