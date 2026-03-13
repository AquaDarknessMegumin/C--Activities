using ToDo.Models;
using ToDo.Services;

namespace ToDo.Views;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class EditCompletedToDoPage : ContentPage
{
    private ToDoClass _currentItem;

    public string ItemId { get; set; }

    public EditCompletedToDoPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (int.TryParse(ItemId, out int id))
        {
            _currentItem = ToDoService.GetItemById(id);
            if (_currentItem != null)
            {
                ItemNameEntry.Text = _currentItem.item_name;
                ItemDescriptionEditor.Text = _currentItem.item_description;
            }
        }
    }

    private async void OnUpdateClicked(object sender, EventArgs e)
    {
        if (_currentItem == null) return;

        string name = ItemNameEntry.Text?.Trim();
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Please enter a task name.", "OK");
            return;
        }

        _currentItem.item_name = name;
        _currentItem.item_description = ItemDescriptionEditor.Text?.Trim() ?? "";
        ToDoService.UpdateItem(_currentItem);

        await DisplayAlert("Success", "Task updated.", "OK");
        await Shell.Current.GoToAsync("..");
    }

    private async void OnMarkActiveClicked(object sender, EventArgs e)
    {
        if (_currentItem == null) return;

        _currentItem.status = "Active";
        ToDoService.UpdateItem(_currentItem);

        await DisplayAlert("Success", "Task marked as active.", "OK");
        await Shell.Current.GoToAsync("..");
    }
}
