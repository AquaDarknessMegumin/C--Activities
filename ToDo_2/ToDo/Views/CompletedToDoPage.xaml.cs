using System.Collections.ObjectModel;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Views;

public partial class CompletedToDoPage : ContentPage
{
    private ObservableCollection<ToDoClass> _completedItems;

    public CompletedToDoPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadItems();
    }

    private async Task LoadItems()
    {
        var user = UserService.GetCurrentUser();
        if (user != null)
        {
            _completedItems = await ToDoService.GetCompletedItems(user.id);
            CompletedListView.ItemsSource = _completedItems;
            EmptyLabel.IsVisible = _completedItems.Count == 0;
        }
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is ToDoClass selectedItem)
        {
            CompletedListView.SelectedItem = null;
            await Shell.Current.GoToAsync($"{nameof(EditCompletedToDoPage)}?itemId={selectedItem.item_id}&itemName={Uri.EscapeDataString(selectedItem.item_name ?? "")}&itemDescription={Uri.EscapeDataString(selectedItem.item_description ?? "")}");
        }
    }
}
