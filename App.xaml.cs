using FateRank.Views;
using FateRank.ViewModels;
namespace FateRank;

public partial class App : Application
{
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