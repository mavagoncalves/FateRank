using System.Windows.Input;
using FateRank.Logic;

namespace FateRank.ViewModels;

/// <summary>
/// The ViewModel for the main game screen.
/// It bridges the View (UI) and the Model (GameEngine).
/// </summary>
public class MainViewModel : BaseViewModel
{
    private readonly GameEngine _engine;
    
    // BACKING FIELDS (The actual data)
    private string _playerImage;
    private string _computerImage;
    private string _playerScore;
    private string _computerScore;
    private string _statusText;
    private bool _isWarVisible;
    private bool _isBusy;
    private bool _isGameOver;

    // PUBLIC PROPERTIES
    public string PlayerImage
    {
        get => _playerImage;
        set { _playerImage = value; OnPropertyChanged(); }
    }

    public string ComputerImage
    {
        get => _computerImage;
        set { _computerImage = value; OnPropertyChanged(); }
    }

    public string PlayerScore
    {
        get => _playerScore;
        set { _playerScore = value; OnPropertyChanged(); }
    }

    public string ComputerScore
    {
        get => _computerScore;
        set { _computerScore = value; OnPropertyChanged(); }
    }

    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    public bool IsWarVisible
    {
        get => _isWarVisible;
        set { _isWarVisible = value; OnPropertyChanged(); }
    }
    
    // Controls if the buttons are clickable
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public bool IsGameOver
    {
        get => _isGameOver;
        set { _isGameOver = value; OnPropertyChanged(); }
    }

    // COMMANDS (The clicks)
    public ICommand DealCommand { get; }
    public ICommand RestartCommand { get; }

    /// <summary>
    /// Initializes the ViewModel and starts a new game.
    /// </summary>
    public MainViewModel()
    {
        _engine = new GameEngine();
        
        // Initialize Commands
        DealCommand = new Command(async () => await PlayTurn());
        RestartCommand = new Command(StartNewGame);

        StartNewGame();
    }

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

    private async Task PlayTurn()
    {
        if (IsBusy) return;
        IsBusy = true; // Lock the button

        Card pCard, cCard;
        string result = _engine.PlayRound(out pCard, out cCard);

        // Update UI
        PlayerImage = pCard?.ImageSource;
        ComputerImage = cCard?.ImageSource;
        StatusText = result;

        if (result == "WAR!")
        {
            await HandleWarLoop(result);
        }

        UpdateScores();
        CheckForWinner();
        
        if (!IsGameOver) IsBusy = false; // Unlock button
    }

    private async Task HandleWarLoop(string result)
    {
        while (result == "WAR!")
        {
            await Task.Delay(2000);
            IsWarVisible = true;
            StatusText = "WAR DETECTED!";
            
            await Task.Delay(2000);
            StatusText = "DEALING 3 FACE-DOWN CARDS...";
            PlayerImage = "card_back.png";
            ComputerImage = "card_back.png";
            
            await Task.Delay(2500);
            
            var pool = new List<Card>(); 
            result = _engine.ExecuteWar(pool, out Card pWar, out Card cWar);

            if (pWar == null || cWar == null)
            {
                StatusText = result;
                CheckForWinner();
                return;
            }

            PlayerImage = pWar.ImageSource;
            ComputerImage = cWar.ImageSource;
            StatusText = result;
        }

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
