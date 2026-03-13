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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadItems();
    }

    private void LoadItems()
    {
        var user = UserService.GetCurrentUser();
        if (user != null)
        {
            _completedItems = ToDoService.GetCompletedItems(user.UserId);
            CompletedListView.ItemsSource = _completedItems;
            EmptyLabel.IsVisible = _completedItems.Count == 0;
        }
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is ToDoClass selectedItem)
        {
            CompletedListView.SelectedItem = null;
            await Shell.Current.GoToAsync($"{nameof(EditCompletedToDoPage)}?itemId={selectedItem.item_id}");
        }
    }
}
