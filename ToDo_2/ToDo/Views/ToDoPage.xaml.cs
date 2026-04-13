using System.Collections.ObjectModel;
using ToDo.Models;
using ToDo.Services;

namespace ToDo.Views;

public partial class ToDoPage : ContentPage
{
    private ObservableCollection<ToDoClass> _activeItems;

    public ToDoPage()
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
            _activeItems = await ToDoService.GetActiveItems(user.id);
            ToDoListView.ItemsSource = _activeItems;
            EmptyLabel.IsVisible = _activeItems.Count == 0;
        }
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is ToDoClass selectedItem)
        {
            ToDoListView.SelectedItem = null;
            await Shell.Current.GoToAsync($"{nameof(EditToDoPage)}?itemId={selectedItem.item_id}&itemName={Uri.EscapeDataString(selectedItem.item_name ?? "")}&itemDescription={Uri.EscapeDataString(selectedItem.item_description ?? "")}");
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddToDoPage));
    }
}
