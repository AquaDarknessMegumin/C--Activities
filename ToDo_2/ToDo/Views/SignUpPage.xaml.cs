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
        string firstName = FirstNameEntry.Text?.Trim();
        string lastName = LastNameEntry.Text?.Trim();
        string email = EmailEntry.Text?.Trim();
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        var (success, message) = await UserService.Register(firstName, lastName, email, password, confirmPassword);
        if (success)
        {
            await DisplayAlert("Success", message, "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", message, "OK");
        }
    }

    private async void OnSignInTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
