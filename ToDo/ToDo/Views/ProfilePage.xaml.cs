using ToDo.Services;

namespace ToDo.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var user = UserService.GetCurrentUser();
        if (user != null)
        {
            NameLabel.Text = user.Name;
            EmailLabel.Text = user.Email;
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (confirm)
        {
            UserService.Logout();
            Application.Current.MainPage = new NavigationPage(new SignInPage());
        }
    }
}
