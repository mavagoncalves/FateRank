using System;
using System.Collections.Generic;

namespace FateRank.Logic;

/// <summary>
/// Represents a standard deck of playing cards.
/// Responsible for creating the initial set of cards, shuffling them, and dealing them out.
/// </summary>
public class Deck
{
    // The internal list of cards currently in the deck.
    private List<Card> _cards = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class.
    /// Automatically generates a full 52-card deck upon creation.
    /// </summary>
    public Deck()
    {
        // Loop through every Suit (Clubs, Diamonds, Hearts, Spades)
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
        {
            // Loop through every Rank (2 through Ace)
            foreach (Rank r in Enum.GetValues(typeof(Rank)))
            {
                _cards.Add(new Card(r, s));
            }
        }
    }

    /// <summary>
    /// Randomizes the order of the cards in the deck using the Fisher-Yates shuffle algorithm.
    /// </summary>
    public void Shuffle()
    {
        Random rng = new Random();
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            // Swap the cards
            Card value = _cards[k];
            _cards[k] = _cards[n];
            _cards[n] = value;
        }
    }

    /// <summary>
    /// Removes and returns the top card from the deck.
    /// </summary>
    /// <returns>The top <see cref="Card"/>, or null if the deck is empty.</returns>
    public Card DealCard()
    {
        if (_cards.Count == 0) return null;

        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}