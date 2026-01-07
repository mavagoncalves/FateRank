namespace FateRank;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // This tells the app to load AppShell, which then loads the MainPage
        MainPage = new AppShell(); 
    }
}