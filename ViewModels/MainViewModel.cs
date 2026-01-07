using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Devices;
using System.Windows.Input;
using FateRank.Logic;

namespace FateRank.ViewModels;

public class MainViewModel : BaseViewModel
{
    /// <summary>
    /// Handles all the UI connections with the game logic
    /// </summary>
    private readonly GameEngine _engine;

    private string _playerImage = "card_back.png";
    private string _computerImage = "card_back.png";
    private string _playerScore = "Deck: 27";
    private string _computerScore = "Deck: 27";
    private string _statusText = "Ready?";
    
    private bool _isWarVisible;
    private bool _isBusy;
    private bool _isGameOver;

    /// <summary>
    /// Gets or sets the source image filename for the player's visible card.
    /// </summary>
    public string PlayerImage
    {
        get => _playerImage;
        set { _playerImage = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the source image filename for the computer's visible card.
    /// </summary>
    public string ComputerImage
    {
        get => _computerImage;
        set { _computerImage = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the player's score text shown in the UI (e.g., "Deck: 27").
    /// </summary>
    public string PlayerScore
    {
        get => _playerScore;
        set { _playerScore = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the computer's score text shown in the UI (e.g., "Deck: 27").
    /// </summary>
    public string ComputerScore
    {
        get => _computerScore;
        set { _computerScore = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Gets or sets the current status text displayed to the user.
    /// </summary>
    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Controls visibility of the war UI element.
    /// </summary>
    public bool IsWarVisible
    {
        get => _isWarVisible;
        set { _isWarVisible = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Indicates whether the UI is currently busy (e.g., playing a turn).
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set 
        { 
            _isBusy = value; 
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotBusy)); 
        }
    }

    /// <summary>
    /// Convenience inverse of <see cref="IsBusy"/> for binding.
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// Indicates whether the game has ended.
    /// </summary>
    public bool IsGameOver
    {
        get => _isGameOver;
        set { _isGameOver = value; OnPropertyChanged(); }
    }

    /// <summary>
    /// Command bound to the Deal/Play action in the UI.
    /// </summary>
    public ICommand DealCommand { get; }

    /// <summary>
    /// Command bound to the Restart action in the UI.
    /// </summary>
    public ICommand RestartCommand { get; }

    /// <summary>
    /// Initializes the main view model, configures commands, and starts a new game.
    /// </summary>
    public MainViewModel()
    {
        _engine = new GameEngine();
        DealCommand = new Command(async () => await PlayTurn());
        RestartCommand = new Command(StartNewGame);
        
        StartNewGame();
    }

    /// <summary>
    /// Resets engine state and UI-bound properties to the initial new-game values.
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
    /// Plays a single turn: invokes the engine, updates image and status properties, and handles wars when they occur.
    /// </summary>
    private async Task PlayTurn()
    {
        if (IsBusy) return;
        IsBusy = true;

        Card pCard, cCard;
        // Engine handles the logic using the Player class internally
        string result = _engine.PlayRound(out pCard, out cCard);

        PlayerImage = pCard.ImageSource;
        ComputerImage = cCard.ImageSource;
        StatusText = result;

        if (result == "WAR!")
        {
            await HandleWarLoop(result, pCard, cCard);
        }

        UpdateScores();
        CheckForWinner();

        if (!IsGameOver) IsBusy = false;
    }

    /// <summary>
    /// Handles the asynchronous war loop by staking cards into the war pool and resolving wars until a winner is found.
    /// </summary>
    /// <param name="result">The initial result string indicating a war.</param>
    /// <param name="initialPCard">The player's card that started the war.</param>
    /// <param name="initialCCard">The computer's card that started the war.</param>
    private async Task HandleWarLoop(string result, Card initialPCard, Card initialCCard)
    {
        var warPool = new List<Card> { initialPCard, initialCCard };

        while (result == "WAR!")
        {
            await Task.Delay(1500);
            IsWarVisible = true;
            StatusText = "WAR DETECTED!";
            
            await Task.Delay(1500);

            // Check if they have enough cards (Need at least 1 hidden + 1 battle = 2)
            if (_engine.PlayerCardCount < 2 || _engine.ComputerCardCount < 2)
            {
                _engine.ForceGameOver();
                CheckForWinner();
                return;
            }

            StatusText = "STAKING CARDS...";
            PlayerImage = "card_back.png";
            ComputerImage = "card_back.png";
            
            await Task.Delay(1500);

            result = _engine.ExecuteWar(warPool, out Card pWar, out Card cWar);

            if (pWar != null) PlayerImage = pWar.ImageSource;
            if (cWar != null) ComputerImage = cWar.ImageSource;
            
            StatusText = result;
        }

        await Task.Delay(2000);
        IsWarVisible = false;
    }

    /// <summary>
    /// Updates the UI-bound score strings from the engine's current player card counts.
    /// </summary>
    private void UpdateScores()
    {
        // Now accessing properties that redirect to the Player class
        PlayerScore = $"Deck: {_engine.PlayerCardCount}";
        ComputerScore = $"Deck: {_engine.ComputerCardCount}";
    }

    /// <summary>
    /// Evaluates if either player has reached a terminal win/lose condition and sets the game-over state.
    /// </summary>
    private void CheckForWinner()
    {
        // 54 total cards in deck
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

    public double CardWidth => Math.Min(200, DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.35);
    public double CardHeight => CardWidth * 1.375;
}
