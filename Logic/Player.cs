using System;
using System.Collections.Generic;
using System.Linq;

namespace FateRank.Logic;

/// <summary>
/// Represents a player in the game.
/// Manages a personal queue of cards (hand) and handles logic for playing and receiving cards.
/// </summary>
public class Player
{
    /// <summary>
    /// Gets the name of the player (e.g., "You" or "Computer").
    /// </summary>
    public string Name { get; private set; }

    // Internal queue to manage the FIFO (First-In, First-Out) nature of the deck
    private Queue<Card> _hand = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="name">The display name of the player.</param>
    public Player(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the current total number of cards in the player's hand.
    /// </summary>
    /// <returns>The count of cards.</returns>
    public int GetCardCount() => _hand.Count;

    /// <summary>
    /// Plays the top card from the player's hand.
    /// </summary>
    /// <returns>The played <see cref="Card"/>, or null if the hand is empty.</returns>
    public Card PlayCard()
    {
        if (_hand.Count > 0)
        {
            return _hand.Dequeue();
        }
        return null;
    }

    /// <summary>
    /// Adds a collection of won cards to the bottom of the player's deck.
    /// The input cards are shuffled before adding to prevent infinite loops in the game logic.
    /// </summary>
    /// <param name="cards">The list of cards to add to the hand.</param>
    public void ReceiveCard(IEnumerable<Card> cards)
    {
        var list = cards.ToList();

        // Basic shuffle of the won loot pile.
        // This is crucial in War to prevent the exact same sequence of cards 
        // from repeating endlessly (a common issue in digital War games).
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }

        // Add the shuffled cards to the bottom of the hand queue
        foreach (var card in list)
        {
            _hand.Enqueue(card);
        }
    }
}