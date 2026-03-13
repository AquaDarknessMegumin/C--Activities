using ToDo.Services;

namespace ToDo.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        bool success = UserService.Register(name, email, password);
        if (success)
        {
            await DisplayAlert("Success", "Account created successfully!", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "An account with this email already exists.", "OK");
        }
    }

    private async void OnSignInTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
