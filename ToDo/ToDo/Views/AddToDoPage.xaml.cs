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

        var newItem = new ToDoClass
        {
            item_name = name,
            item_description = description ?? "",
            user_id = user.UserId
        };

        ToDoService.AddItem(newItem);
        await Shell.Current.GoToAsync("..");
    }
}
