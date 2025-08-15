using DentalHealthFollow_Up.Shared.DTOs;
using Microsoft.Maui.Storage;
using System.Net.Http.Json;

namespace DentalHealthFollow_Up.MAUI
{
    public partial class GoalEntryPage : ContentPage
    {
        private int CurrentUserId => Preferences.Get("CurrentUserId", 0);

        public GoalEntryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CurrentUserId <= 0)
            {
                await DisplayAlert("Uyar�", "�nce giri� yap�n.", "Tamam");
                return;
            }
            await LoadGoals();
        }

        private async Task LoadGoals()
        {
            var list = await Api.Client()
                .GetFromJsonAsync<List<GoalDto>>($"api/Goals/user/{CurrentUserId}");

            GoalsCollection.ItemsSource = list ?? new();
        }

        private async void OnSaveGoalClicked(object sender, EventArgs e)
        {
            try
            {
                var title = TitleEntry.Text?.Trim();
                if (string.IsNullOrWhiteSpace(title))
                { await DisplayAlert("Uyar�", "Ba�l�k zorunludur.", "Tamam"); return; }

                int? targetMinutes = null; 

                var payload = new
                {
                    UserId = Preferences.Get("CurrentUserId", 0),
                    Title = title!,
                    TargetDurationInMinutes = targetMinutes 
                };

                var resp = await Api.Client().PostAsJsonAsync("api/Goals", payload);
                if (resp.IsSuccessStatusCode)
                {
                    await DisplayAlert("Ba�ar�l�", "Hedef kaydedildi.", "Tamam");
                    TitleEntry.Text = string.Empty;
                    DescriptionEditor.Text = string.Empty;
                    PeriodPicker.SelectedIndex = -1;
                    ImportancePicker.SelectedIndex = -1;
                    await LoadGoals();
                }
                else
                {
                    var msg = await resp.Content.ReadAsStringAsync();
                    await DisplayAlert("Hata", string.IsNullOrWhiteSpace(msg) ? "Kay�t ba�ar�s�z." : msg, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }


        private async void OnDeleteGoalClicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.CommandParameter is int id && id > 0)
                {
                    var ok = await DisplayAlert("Onay", "Se�ilen hedef silinsin mi?", "Evet", "Hay�r");
                    if (!ok) return;

                    var resp = await Api.Client().DeleteAsync($"api/Goals/{id}");
                    if (resp.IsSuccessStatusCode)
                    {
                        await LoadGoals();
                        await DisplayAlert("Bilgi", "Silindi.", "Tamam");
                    }
                    else
                    {
                        var msg = await resp.Content.ReadAsStringAsync();
                        await DisplayAlert("Hata", string.IsNullOrWhiteSpace(msg) ? "Silme ba�ar�s�z." : msg, "Tamam");
                    }
                }
                else
                {
                    await DisplayAlert("Uyar�", "Silmek i�in listedeki 'Sil' butonuna bas�n.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
}

