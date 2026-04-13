using ToDo.Views;

namespace ToDo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for pages that are navigated to programmatically
        Routing.RegisterRoute(nameof(AddToDoPage), typeof(AddToDoPage));
        Routing.RegisterRoute(nameof(EditToDoPage), typeof(EditToDoPage));
        Routing.RegisterRoute(nameof(EditCompletedToDoPage), typeof(EditCompletedToDoPage));
    }
}