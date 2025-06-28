using Microsoft.Maui.Controls;

namespace fuzzy
{
    [QueryProperty(nameof(Username), "username")]
    public partial class Menu : ContentPage
    {
        private string username;

        public string Username
        {
            get => username;
            set
            {
                username = Uri.UnescapeDataString(value);
                // You can display it or use it here if needed
            }
        }

        public Menu()
        {
            InitializeComponent();
        }

        private async void OnDashboardClicked(object sender, EventArgs e)
        {
            // Pass username along to Dashboard
            await Shell.Current.GoToAsync($"{nameof(Dashboard)}?username={Uri.EscapeDataString(username)}");
        }

        private async void OnUserInfoClicked(object sender, EventArgs e)
        {
            // Pass username along to UserInfo
            await Shell.Current.GoToAsync($"{nameof(UserInfo)}?username={Uri.EscapeDataString(username)}");
        }
    }

}
