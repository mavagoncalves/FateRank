using System;
using System.Collections.Generic;
using System.Linq;

namespace FateRank.Logic;

/// <summary>
/// Represents a player in the game.
/// Manages a personal queue of cards (hand) and handles receiving winnings.
/// </summary>
public class Player
{
    /// <summary>
    /// Gets the name of the player.
    /// </summary>
    public string Name { get; private set; }

    private Queue<Card> _hand = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="name">The player's name.</param>
    public Player(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the current number of cards in the player's hand.
    /// </summary>
    public int GetCardCount() => _hand.Count;

    /// <summary>
    /// Plays the top card from the player's hand.
    /// </summary>
    /// <returns>The card, or null if empty.</returns>
    public Card PlayCard()
    {
        return _hand.Count > 0 ? _hand.Dequeue() : null;
    }

    /// <summary>
    /// Adds won cards to the bottom of the player's deck.
    /// Shuffles the input list before adding to prevent infinite loop patterns.
    /// </summary>
    /// <param name="cards">The list of won cards.</param>
    public void ReceiveCard(IEnumerable<Card> cards)
    {
        var list = cards.ToList();

        // Shuffle loot to prevent game loops
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        foreach (var card in list)
        {
            _hand.Enqueue(card);
        }
    }
}