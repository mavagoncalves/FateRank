namespace FateRank.Logic;

/// <summary>
/// Manages the game flow by coordinating two Player objects and a Deck.
/// </summary>
public class GameEngine
{
    //Engine has Players and a Deck
    private Player _player1;
    private Player _computer;
    private Deck _deck;

    // Public properties for the ViewModel to bind to
    public int PlayerCardCount => _player1.GetCardCount();
    public int ComputerCardCount => _computer.GetCardCount();

    /// <summary>
    /// Creates players, deck, shuffles, and deals.
    /// </summary>
    public void InitializeGame()
    {
        _player1 = new Player("You");
        _computer = new Player("Computer");
        _deck = new Deck(); // Uses the new Deck class
        
        _deck.Shuffle();

        // Deal logic (Using the Deck class)
        bool dealToPlayer = true;
        Card drawn;
        while ((drawn = _deck.DealCard()) != null)
        {
            // Uses the Player class to receive cards
            if (dealToPlayer) _player1.ReceiveCard(new[] { drawn });
            else _computer.ReceiveCard(new[] { drawn });
            
            dealToPlayer = !dealToPlayer;
        }
    }

    /// <summary>
    /// Plays one round. Delegates card management to the Player classes.
    /// </summary>
    public string PlayRound(out Card pCard, out Card cCard)
    {
        // 1Ask Players to play a card
        pCard = _player1.PlayCard();
        cCard = _computer.PlayCard();

        // Check for Game Over immediately
        if (pCard == null || cCard == null) return "Game Over";

        // Compare
        int comparison = pCard.CompareTo(cCard);
        var loot = new List<Card> { pCard, cCard };

        if (comparison > 0)
        {
            _player1.ReceiveCard(loot); // Give loot to Player
            return "YOU WIN THIS ROUND!";
        }
        else if (comparison < 0)
        {
            _computer.ReceiveCard(loot); // Give loot to Computer
            return "CPU WINS THIS ROUND!";
        }
        else
        {
            return "WAR!"; // ViewModel handles the UI loop for this
        }
    }

    /// <summary>
    /// Handles the War logic using Player classes.
    /// Accepts 'currentPool' to track cards visible in the UI.
    /// </summary>
    public string ExecuteWar(List<Card> currentPool, out Card pWar, out Card cWar)
    {
        // Draw the deciding cards (Face Up)
        pWar = _player1.PlayCard();
        cWar = _computer.PlayCard();
        
        // Simulate drawing 3 hidden cards per player
        for(int i=0; i<3; i++) {
             Card p = _player1.PlayCard();
             Card c = _computer.PlayCard();
             
             // Add hidden cards to the pool so the winner gets them
             if(p != null) currentPool.Add(p);
             if(c != null) currentPool.Add(c);
        }

        // Add the deciding cards to the pool
        if (pWar != null) currentPool.Add(pWar);
        if (cWar != null) currentPool.Add(cWar);

        // Check for Game Over (Run out of cards during war)
        if (pWar == null || cWar == null) return "Not enough cards!";

        // 5. Compare the War cards
        int comparison = pWar.CompareTo(cWar);
        
        if (comparison > 0)
        {
            _player1.ReceiveCard(currentPool);
            return "YOU WON THE WAR!";
        }
        else if (comparison < 0)
        {
            _computer.ReceiveCard(currentPool);
            return "CPU WON THE WAR!";
        }
        
        return "WAR!"; // Double War!
    }
}