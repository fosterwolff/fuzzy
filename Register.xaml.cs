using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace fuzzy;

public partial class Register : ContentPage
{
    public Register()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string confirmEmail = ConfirmEmailEntry.Text?.Trim();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(confirmEmail) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (email != confirmEmail)
        {
            await DisplayAlert("Error", "Emails do not match.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        string hashedPassword = ComputeSha256Hash(password);

        var registerData = new
        {
            username = username,
            email = email,
            password = hashedPassword
        };

        try
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://127.0.0.1:5001"); // Update if using physical device

            HttpResponseMessage response = await client.PostAsJsonAsync("/register", registerData);

            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.GoToAsync(nameof(Dashboard));
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Failed", $"Registration failed: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private string ComputeSha256Hash(string rawData)
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
