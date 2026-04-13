using ToDo.Models;
using ToDo.Services;

namespace ToDo.Views;

[QueryProperty(nameof(ItemId), "itemId")]
[QueryProperty(nameof(ItemName), "itemName")]
[QueryProperty(nameof(ItemDescription), "itemDescription")]
public partial class EditToDoPage : ContentPage
{
    private int _itemId;

    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemDescription { get; set; }

    public EditToDoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (int.TryParse(ItemId, out int id))
        {
            _itemId = id;
            ItemNameEntry.Text = Uri.UnescapeDataString(ItemName ?? "");
            ItemDescriptionEditor.Text = Uri.UnescapeDataString(ItemDescription ?? "");
        }
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        string name = ItemNameEntry.Text?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Please enter a task name.", "OK");
            return;
        }

        var (success, message) = await ToDoService.UpdateItem(_itemId, name, ItemDescriptionEditor.Text?.Trim() ?? "");

        if (success)
        {
            await DisplayAlert("Success", message, "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", message, "OK");
        }
    }

    private async void OnMarkCompletedClicked(object sender, EventArgs e)
    {
        var (success, message) = await ToDoService.ChangeStatus(_itemId, "inactive");

        if (success)
        {
            await DisplayAlert("Success", message, "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", message, "OK");
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete", "Are you sure you want to delete this task?", "Yes", "No");
        if (!confirm) return;

        var (success, message) = await ToDoService.DeleteItem(_itemId);

        if (success)
        {
            await DisplayAlert("Success", message, "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Error", message, "OK");
        }
    }
}
