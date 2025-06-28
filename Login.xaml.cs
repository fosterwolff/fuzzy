namespace fuzzy;

using System.Security.Cryptography;
using System.Text;

using System.Net.Http;
using System.Net.Http.Json;


public partial class Login : ContentPage
{
    public Login()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            string hashedPassword = ComputeSha256Hash(password);

            var loginData = new
            {
                username = username,
                password = hashedPassword
            };

            System.Diagnostics.Debug.WriteLine($"Sending login data: {System.Text.Json.JsonSerializer.Serialize(loginData)}");

            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://127.0.0.1:5001"); // or emulator IP

            HttpResponseMessage response = await client.PostAsJsonAsync("/login", loginData);

            string responseText = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine($"Server response: {response.StatusCode} - {responseText}");

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync($"{nameof(Menu)}?username={Uri.EscapeDataString(username)}");
            }
            else
            {
                await DisplayAlert("Failed", $"Login failed. {responseText}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }


    string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }

}
