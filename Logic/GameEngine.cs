using System.Collections.Generic;
using FateRank.Models; 

namespace FateRank.Logic;

/// <summary>
/// Manages the core game logic, coordinating Players and the Deck.
/// Follows MVVM architecture by exposing methods for the ViewModel.
/// </summary>
public class GameEngine
{
    private Player _player1;
    private Player _computer;
    private Deck _deck;

    /// <summary>
    /// Gets the number of cards held by the human player.
    /// </summary>
    public int PlayerCardCount => _player1.GetCardCount();

    /// <summary>
    /// Gets the number of cards held by the computer.
    /// </summary>
    public int ComputerCardCount => _computer.GetCardCount();

    /// <summary>
    /// Initializes the game: creates players, shuffles deck, and deals cards.
    /// </summary>
    public void InitializeGame()
    {
        _player1 = new Player("You");
        _computer = new Player("Computer");
        _deck = new Deck();

        _deck.Shuffle();

        bool dealToPlayer = true;
        Card drawn;
        while ((drawn = _deck.DealCard()) != null)
        {
            if (dealToPlayer) _player1.ReceiveCard(new[] { drawn });
            else _computer.ReceiveCard(new[] { drawn });

            dealToPlayer = !dealToPlayer;
        }
    }

    /// <summary>
    /// Executes a single round of play.
    /// </summary>
    /// <param name="pCard">The card played by the human.</param>
    /// <param name="cCard">The card played by the computer.</param>
    /// <returns>A status string (Win/Loss/War).</returns>
    public string PlayRound(out Card pCard, out Card cCard)
    {
        pCard = _player1.PlayCard();
        cCard = _computer.PlayCard();

        if (pCard == null || cCard == null) return "Game Over";

        int comparison = pCard.CompareTo(cCard);
        var loot = new List<Card> { pCard, cCard };

        if (comparison > 0)
        {
            _player1.ReceiveCard(loot);
            return "YOU WIN THIS ROUND!";
        }
        else if (comparison < 0)
        {
            _computer.ReceiveCard(loot);
            return "CPU WINS THIS ROUND!";
        }
        else
        {
            return "WAR!";
        }
    }

    /// <summary>
    /// Handles the War scenario logic.
    /// </summary>
    /// <param name="currentPool">The list of cards currently at stake.</param>
    /// <param name="pWar">The visible card played by the human.</param>
    /// <param name="cWar">The visible card played by the computer.</param>
    /// <returns>A status string indicating the War result.</returns>
    public string ExecuteWar(List<Card> currentPool, out Card pWar, out Card cWar)
    {
        pWar = _player1.PlayCard();
        cWar = _computer.PlayCard();

        // Burn 3 cards (Face down)
        for (int i = 0; i < 3; i++)
        {
            Card p = _player1.PlayCard();
            Card c = _computer.PlayCard();
            if (p != null) currentPool.Add(p);
            if (c != null) currentPool.Add(c);
        }

        // Add the face-up deciding cards
        if (pWar != null) currentPool.Add(pWar);
        if (cWar != null) currentPool.Add(cWar);

        if (pWar == null || cWar == null) return "Not enough cards!";

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

        return "WAR!";
    }
}