using System.Collections.ObjectModel;
using YourProjectName.Models;

namespace YourProjectName;

public partial class MainPage : ContentPage
{
    ObservableCollection<ToDoItem> toDoItems;
    ToDoItem selectedItem;

    public MainPage()
    {
        InitializeComponent();

        toDoItems = new ObservableCollection<ToDoItem>();
        ToDoListView.ItemsSource = toDoItems;
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            return;

        if (selectedItem == null)
        {
            // ADD
            toDoItems.Add(new ToDoItem
            {
                Title = TitleEntry.Text,
                Details = DetailsEntry.Text
            });
        }
        else
        {
            // EDIT
            selectedItem.Title = TitleEntry.Text;
            selectedItem.Details = DetailsEntry.Text;
            selectedItem = null;
        }

        TitleEntry.Text = "";
        DetailsEntry.Text = "";
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        selectedItem = e.SelectedItem as ToDoItem;

        TitleEntry.Text = selectedItem.Title;
        DetailsEntry.Text = selectedItem.Details;
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (selectedItem != null)
        {
            toDoItems.Remove(selectedItem);
            selectedItem = null;

            TitleEntry.Text = "";
            DetailsEntry.Text = "";
        }
    }
}