using fuzzy;

namespace fuzzy;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent(); // this connects to the XAML
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Register");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Login");
    }



}
