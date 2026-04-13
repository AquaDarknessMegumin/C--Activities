using ToDo.Models;
using ToDo.Services;

namespace ToDo.Views;

public partial class AddToDoPage : ContentPage
{
    public AddToDoPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        string name = ItemNameEntry.Text?.Trim();
        string description = ItemDescriptionEditor.Text?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Please enter a task name.", "OK");
            return;
        }

        var user = UserService.GetCurrentUser();
        if (user == null) return;

        var (success, message) = await ToDoService.AddItem(name, description ?? "", user.id);

        if (success)
        {
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", message, "OK");
        }
    }
}
