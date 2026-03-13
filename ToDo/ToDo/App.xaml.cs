using ToDo.Views;

namespace ToDo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Start with Sign In page wrapped in NavigationPage
        return new Window(new NavigationPage(new SignInPage()));
    }
}