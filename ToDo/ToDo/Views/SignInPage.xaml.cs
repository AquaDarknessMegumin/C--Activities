using ToDo.Services;

namespace ToDo.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter email and password.", "OK");
            return;
        }

        bool success = UserService.Login(email, password);
        if (success)
        {
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            await DisplayAlert("Error", "Invalid email or password.", "OK");
        }
    }

    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }
}
