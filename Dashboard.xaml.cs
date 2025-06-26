using System.Text.Json;

namespace fuzzy;

public partial class Dashboard : ContentPage
{
    public Dashboard()
    {
        InitializeComponent();
        // Initialize to current date/time if you want
        EventDatePicker.Date = DateTime.Now.Date;
        EventTimePicker.Time = DateTime.Now.TimeOfDay;
    }

    private void OnCreateEventClicked(object sender, EventArgs e)
    {
        var selectedDate = EventDatePicker.Date;
        var selectedTime = EventTimePicker.Time;
        var note = NoteEditor.Text?.Trim() ?? "";

        var eventDateTime = selectedDate.Add(selectedTime);

        var newEvent = new
        {
            date = eventDateTime.ToString("yyyy-MM-dd"),
            time = eventDateTime.ToString("HH:mm:ss"),
            note = note
        };

        // Serialize to JSON (for example)
        string eventJson = JsonSerializer.Serialize(newEvent);

        // For now, show JSON in label and make it visible
        ResultLabel.Text = $"Created Event JSON:\n{eventJson}";
        ResultLabel.IsVisible = true;

        // TODO: send eventJson to your server via HTTP POST later
    }
}
