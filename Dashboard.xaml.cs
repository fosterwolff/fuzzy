using System;
using System.Net.Http.Json;

namespace fuzzy;

[QueryProperty(nameof(Username), "username")]
public partial class Dashboard : ContentPage
{
    public string Username { get; set; }

    public Dashboard()
    {
        InitializeComponent();

        // Start timer to update clock every second
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            TimeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            return true; // Repeat
        });
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Update welcome message with username
        WelcomeLabel.Text = $"Welcome, {Username}";
    }

    private async void OnCreateEventClicked(object sender, EventArgs e)
    {
        var selectedDate = EventDatePicker.Date;
        var selectedTime = EventTimePicker.Time;
        var note = NoteEditor.Text?.Trim() ?? "";

        var newEvent = new
        {
            username = Username,
            date = selectedDate.ToString("yyyy-MM-dd"),
            time = selectedTime.ToString(@"hh\:mm\:ss"),
            note = note
        };

        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("http://127.0.0.1:5001");

            HttpResponseMessage response = await client.PostAsJsonAsync("/create_event", newEvent);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ResultLabel.Text = $"Event created:\n{result}";
                ResultLabel.TextColor = Colors.Green;
            }
            else
            {
                ResultLabel.Text = $"Failed to create event.\n{response.StatusCode}";
                ResultLabel.TextColor = Colors.Red;
            }
        }
        catch (Exception ex)
        {
            ResultLabel.Text = $"Error: {ex.Message}";
            ResultLabel.TextColor = Colors.Red;
        }

        ResultLabel.IsVisible = true;
    }
}
