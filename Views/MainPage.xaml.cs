using FateRank.Logic;
using FateRank.Models;

namespace FateRank.Views;

public partial class MainPage : ContentPage
{
    // Create an instance of your logic
    private GameEngine _engine = new GameEngine();

    public MainPage()
    {
        InitializeComponent();
        _engine.InitializeGame(); // Setup the decks
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        Card pCard, cCard;
    	string result = _engine.PlayRound(out pCard, out cCard);

    	if (pCard != null) 
    	{
        // This will pop up a window on your Mac telling us the filename
        	await DisplayAlert("Debug Info", $"Looking for: {pCard.ImageSource}", "OK");
    	}

    	PlayerCardImage.Source = pCard?.ImageSource;
    	ComputerCardImage.Source = cCard?.ImageSource;

        // Update the counts
        PlayerCountLabel.Text = $"Player: {_engine.PlayerCardCount}";
        ComputerCountLabel.Text = $"CPU: {_engine.ComputerCardCount}";

        // Handle the "War" scenario
        if (result == "WAR!")
        {
            StatusLabel.Text = "IT IS WAR! DRAWING EXTRA CARDS...";
            // We'll call the war logic immediately to resolve it
            result = _engine.ExecuteWar(out pCard, out cCard);
            
            // Show the final cards that decided the war
            PlayerCardImage.Source = pCard?.ImageSource;
            ComputerCardImage.Source = cCard?.ImageSource;
        }

        StatusLabel.Text = result;

        // Check if game is over
        if (result.Contains("Game Over") || _engine.PlayerCardCount == 0 || _engine.ComputerCardCount == 0)
        {
            PlayBtn.IsEnabled = false;
            RestartBtn.IsVisible = true;
        }
    }

    private void OnRestartClicked(object sender, EventArgs e)
    {
        _engine = new GameEngine();
        _engine.InitializeGame();
        PlayBtn.IsEnabled = true;
        RestartBtn.IsVisible = false;
        StatusLabel.Text = "Game Reset! Press Play.";
    }
}
