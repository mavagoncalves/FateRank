using FateRank.Logic;
using FateRank.Models;

namespace FateRank.Views;

public partial class MainPage : ContentPage
{
    private GameEngine _engine = new GameEngine();

    public MainPage()
    {
        InitializeComponent();
        _engine.InitializeGame(); // Setup the decks

		// Show the backs of the cards before the first deal
		PlayerCardImage.Source = "card_back.png";
		ComputerCardImage.Source = "card_back.png";
    }

    private async void OnPlayClicked(object sender, EventArgs e)
	{
		Card pCard, cCard;
		string result = _engine.PlayRound(out pCard, out cCard);

		// Initial reveal of the drawn cards
		PlayerCardImage.Source = pCard?.ImageSource;
		ComputerCardImage.Source = cCard?.ImageSource;
		StatusLabel.Text = result;

		if (result == "WAR!")
		{
			PlayBtn.IsEnabled = false;
			
			// Pause for 2 seconds so the user can see the cards that caused the tie
			await Task.Delay(2000); 

			// Show War Visual and update status
			WarPileVisual.IsVisible = true;
			StatusLabel.Text = "STAKES ARE RISING...";
			await Task.Delay(2500); // 2.5 seconds to process the transition

			StatusLabel.Text = "DEALING 3 FACE-DOWN CARDS...";
			await Task.Delay(2500); // 2.5 seconds for dramatic effect

			// Execute the actual War calculation
			string warResult = _engine.ExecuteWar(out Card pWar, out Card cWar);
			
			// Final reveal of the War outcome
			PlayerCardImage.Source = pWar?.ImageSource;
			ComputerCardImage.Source = cWar?.ImageSource;
			StatusLabel.Text = warResult;

			// Leave the final result on screen for 3 seconds before resetting
			await Task.Delay(3000);
			WarPileVisual.IsVisible = false;
			PlayBtn.IsEnabled = true;
		}

		// Refresh Deck counts
		PlayerCountLabel.Text = $"Deck: {_engine.PlayerCardCount}";
		ComputerCountLabel.Text = $"Deck: {_engine.ComputerCardCount}";

		CheckForWinner();
	}

	private void CheckForWinner()
	{
		// If you have all 54 cards, you win!
		if (_engine.PlayerCardCount >= 54)
		{
			ShowEndGame("CONGRATULATIONS!\nYOU CLEARED THE TABLE");
		}
		// If the computer has all 54 cards, you lose.
		else if (_engine.ComputerCardCount >= 54)
		{
			ShowEndGame("GAME OVER\nTHE HOUSE WINS");
		}
	}

	private void ShowEndGame(string message)
	{
		// This fills the text in your Poker-themed overlay
		WinnerText.Text = message;
		// This makes the dark overlay visible over the table
		GameOverOverlay.IsVisible = true;
		// This stops the user from clicking "Deal" after the game is over
		PlayBtn.IsEnabled = false;
	}
    private void OnRestartClicked(object sender, EventArgs e)
    {
        _engine = new GameEngine();
        _engine.InitializeGame();
        PlayBtn.IsEnabled = true;
        StatusLabel.Text = "Game Reset! Press Play.";
    }
}
