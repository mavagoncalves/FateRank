using FateRank.Views;
using FateRank.ViewModels;
namespace FateRank;

/// <summary>
/// The application class which creates and displays the main page for the app.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the application: sets up the main page and assigns its view model.
    /// </summary>
    public App()
    {
        InitializeComponent();

        var gamePage = new MainPage();

        // Connect the "Brain" to the "Face" manually
        gamePage.BindingContext = new MainViewModel();

        // Force the app to show this page immediately
        MainPage = gamePage;
    }
}