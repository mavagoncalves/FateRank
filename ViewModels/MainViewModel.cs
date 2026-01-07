using System.Windows.Input;
using FateRank.Logic; // Needs access to GameEngine, Card, etc.

namespace FateRank.ViewModels;

/// <summary>
/// The ViewModel for the main game screen.
/// It acts as the bridge between the View (UI) and the Model (GameEngine).
/// It handles button clicks, updates scores, and manages the game state.
/// </summary>
public class MainViewModel : BaseViewModel
{
    // The game logic engine
    private readonly GameEngine _engine;
    
    // --- BACKING FIELDS ---
    private string _playerImage;
    private string _computerImage;
    private string _playerScore;
    private string _computerScore;
    private string _statusText;
    private bool _isWarVisible;
    private bool _isBusy;
    private bool _isGameOver;

    // --- PUBLIC PROPERTIES (The UI binds to these) ---

    /// <summary>
    /// Gets or sets the image filename for the player's current card.
    /// </summary>
    public string PlayerImage
    {
        get => _playerImage;
        set { _playerImage = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the image filename for the computer's current card.
    /// </summary>
    public string ComputerImage
    {
        get => _computerImage;
        set { _computerImage = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the text displaying the player's remaining card count.
    /// </summary>
    public string PlayerScore
    {
        get => _playerScore;
        set { _playerScore = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the text displaying the computer's remaining card count.
    /// </summary>
    public string ComputerScore
    {
        get => _computerScore;
        set { _computerScore = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the status message shown in the center of the screen (e.g., "YOU WIN!").
    /// </summary>
    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Controls the visibility of the "WAR DETECTED" red banner.
    /// </summary>
    public bool IsWarVisible
    {
        get => _isWarVisible;
        set { _isWarVisible = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Indicates if an animation is currently playing.
    /// Used to lock the buttons so the user can't double-click.
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set 
        { 
            _isBusy = value; 
            OnPropertyChanged();
            // Important: Notify that 'IsNotBusy' changed too, so the button knows to enable/disable
            OnPropertyChanged(nameof(IsNotBusy)); 
        }
    }

    /// <summary>
    /// Helper property for binding. Returns TRUE when the game is idle (button should be clickable).
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// Controls the visibility of the "Game Over" overlay screen.
    /// </summary>
    public bool IsGameOver
    {
        get => _isGameOver;
        set { _isGameOver = value; OnPropertyChanged(); }
    }

    // --- COMMANDS (These replace the "Clicked" events) ---
    public ICommand DealCommand { get; }
    public ICommand RestartCommand { get; }

    /// <summary>
    /// Initializes a new instance of the MainViewModel.
    /// Sets up commands and starts the first game.
    /// </summary>
    public MainViewModel()
    {
        _engine = new GameEngine();
        
        // Connect the "DEAL" button to the PlayTurn method
        DealCommand = new Command(async () => await PlayTurn());
        
        // Connect the "NEW GAME" button to the StartNewGame method
        RestartCommand = new Command(StartNewGame);

        StartNewGame();
    }

    /// <summary>
    /// Resets the game engine and the UI to the starting state.
    /// </summary>
    private void StartNewGame()
    {
        _engine.InitializeGame();
        
        PlayerImage = "card_back.png";
        ComputerImage = "card_back.png";
        UpdateScores();
        StatusText = "NEW GAME! DEAL TO START.";
        IsWarVisible = false;
        IsGameOver = false;
        IsBusy = false;
    }

    /// <summary>
    /// Handles the logic for a single turn (Deal -> Compare -> Update UI).
    /// </summary>
    private async Task PlayTurn()
    {
        if (IsBusy) return; // Stop if already running
        IsBusy = true;      // Lock the button

        // 1. Ask Engine to play a round
        Card pCard, cCard;
        string result = _engine.PlayRound(out pCard, out cCard);

        // 2. Update UI
        PlayerImage = pCard?.ImageSource;
        ComputerImage = cCard?.ImageSource;
        StatusText = result;

        // 3. Handle War if necessary
        if (result == "WAR!")
        {
            await HandleWarLoop(result);
        }

        // 4. Update Counts and Check for Winner
        UpdateScores();
        CheckForWinner();
        
        // 5. Unlock the button (only if game isn't over)
        if (!IsGameOver) IsBusy = false; 
    }

    /// <summary>
    /// Handles the async animation loop for the "War" scenario.
    /// </summary>
    private async Task HandleWarLoop(string result)
    {
        // We create a list to track cards in the pot (for the architecture requirement)
        var warPool = new List<Card>();

        while (result == "WAR!")
        {
            await Task.Delay(2000); // Wait to see the tie
            IsWarVisible = true;
            StatusText = "WAR DETECTED!";
            
            await Task.Delay(2000);
            StatusText = "DEALING 3 FACE-DOWN CARDS...";
            PlayerImage = "card_back.png";
            ComputerImage = "card_back.png";
            
            await Task.Delay(2500); // Dramatic pause
            
            // Execute War Logic
            result = _engine.ExecuteWar(warPool, out Card pWar, out Card cWar);

            // Handle "Not enough cards" edge case
            if (pWar == null || cWar == null)
            {
                StatusText = result;
                CheckForWinner();
                return; // Stop immediately
            }

            // Show results
            PlayerImage = pWar.ImageSource;
            ComputerImage = cWar.ImageSource;
            StatusText = result;
        }

        // Cleanup
        await Task.Delay(3000);
        IsWarVisible = false;
    }

    private void UpdateScores()
    {
        PlayerScore = $"Deck: {_engine.PlayerCardCount}";
        ComputerScore = $"Deck: {_engine.ComputerCardCount}";
    }

    private void CheckForWinner()
    {
        if (_engine.PlayerCardCount >= 54 || _engine.ComputerCardCount == 0)
        {
            StatusText = "VICTORY! YOU CLEARED THE TABLE 🏆";
            IsGameOver = true;
        }
        else if (_engine.ComputerCardCount >= 54 || _engine.PlayerCardCount == 0)
        {
            StatusText = "GAME OVER... YOU RAN OUT OF CARDS 💀";
            IsGameOver = true;
        }
    }
}
