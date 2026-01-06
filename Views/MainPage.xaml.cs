using FateRank.Logic;
using FateRank.Models;

namespace FateRank.Views;

public partial class MainPage : ContentPage
{
    private GameEngine _engine = new GameEngine();

    public MainPage()
    {
        InitializeComponent();
        StartNewGame();
    }

	private void StartNewGame()
	{
		// Reset the Engine Logic
		_engine = new Logic.GameEngine();
		_engine.InitializeGame(); 

		// Reset the Visuals
		PlayerCardImage.Source = "card_back.png";
		ComputerCardImage.Source = "card_back.png";
		
		PlayerCountLabel.Text = "Deck: 27";
		ComputerCountLabel.Text = "Deck: 27";
		StatusLabel.Text = "NEW GAME! DEAL TO START.";

		// Reset Game Over State
		GameOverOverlay.IsVisible = false;
		PlayBtn.IsEnabled = true;
		WarPileVisual.IsVisible = false;
	}

    private async void OnPlayClicked(object sender, EventArgs e)
	{
		PlayBtn.IsEnabled = false;
		Card pCard, cCard;
		string result = _engine.PlayRound(out pCard, out cCard);

		// Initial reveal of the drawn cards
		PlayerCardImage.Source = pCard?.ImageSource;
		ComputerCardImage.Source = cCard?.ImageSource;
		StatusLabel.Text = result;

		if (result != "WAR!")
		{
			// If it's NOT War, unlock the button immediately
			if (!GameOverOverlay.IsVisible)
			{
				PlayBtn.IsEnabled = true;
			}
		}
		else 
    	{

			while (result == "WAR!")
			{
				// Pause for 2 seconds so the user can see the cards that caused the tie
				await Task.Delay(2000); 

				// Show War Visual and update status
				WarPileVisual.IsVisible = true;
				StatusLabel.Text = (StatusLabel.Text == "WAR!") ? "WAR DETECTED!" : "DOUBLE WAR DETECTED!";
				await Task.Delay(2000);

				// Turn the cards face-down to signal War has started
				StatusLabel.Text = "DEALING 3 FACE-DOWN CARDS...";
				PlayerCardImage.Source = "card_back.png";
				ComputerCardImage.Source = "card_back.png";
				await Task.Delay(2500); // 2.5 seconds for dramatic effect

				// Execute the actual War calculation
				result = _engine.ExecuteWar(out Card pWar, out Card cWar);

				// check if someone ran out of cards completely during the war
				if (pWar == null || cWar == null)
				{
					StatusLabel.Text = result;
					CheckForWinner();
					return; // STOP
				}
				
				// Final reveal of the War outcome
				PlayerCardImage.Source = pWar?.ImageSource;
				ComputerCardImage.Source = cWar?.ImageSource;
				StatusLabel.Text = result;
			}

			// Leave the final result on screen for 3 seconds before resetting
			await Task.Delay(3000);
			WarPileVisual.IsVisible = false;

			if (!GameOverOverlay.IsVisible)
			{
				PlayBtn.IsEnabled = true;
			}
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
		WinnerText.Text = message;
		// This makes the dark overlay visible over the table
		GameOverOverlay.IsVisible = true;
		// This stops the user from clicking "Deal" after the game is over
		PlayBtn.IsEnabled = false;
	}

	private void OnRestartClicked(object sender, EventArgs e)
	{
		StartNewGame(); 
	}
}
