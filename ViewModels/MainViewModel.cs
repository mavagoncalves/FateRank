using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FateRank.Logic;

namespace FateRank.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly GameEngine _engine;

    private string _playerImage = "card_back.png";
    private string _computerImage = "card_back.png";
    private string _playerScore = "Deck: 27";
    private string _computerScore = "Deck: 27";
    private string _statusText = "Ready?";
    
    private bool _isWarVisible;
    private bool _isBusy;
    private bool _isGameOver;

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

    public bool IsNotBusy => !IsBusy;

    public bool IsGameOver
    {
        get => _isGameOver;
        set { _isGameOver = value; OnPropertyChanged(); }
    }

    public ICommand DealCommand { get; }
    public ICommand RestartCommand { get; }

    public MainViewModel()
    {
        _engine = new GameEngine();
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

    private void UpdateScores()
    {
        // Now accessing properties that redirect to the Player class
        PlayerScore = $"Deck: {_engine.PlayerCardCount}";
        ComputerScore = $"Deck: {_engine.ComputerCardCount}";
    }

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
}
