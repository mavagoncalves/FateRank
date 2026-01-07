using System;
using System.Collections.Generic;

namespace FateRank.Logic;

/// <summary>
/// Represents a standard deck of 54 playing cards (52 standard + 2 Jokers).
/// Responsible for creation, shuffling, and dealing.
/// </summary>
public class Deck
{
    private List<Card> _cards = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Deck"/> class.
    /// Creates 52 standard cards and adds 2 Jokers (Red and Black).
    /// </summary>
    public Deck()
    {
        // 1. Add standard 52 cards
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank r in Enum.GetValues(typeof(Rank)))
            {
                if (r == Rank.Joker) continue; // Skip Joker in main loop
                _cards.Add(new Card(r, s));
            }
        }

        // 2. Add the 2 Jokers
        // Hearts = Red Joker
        _cards.Add(new Card(Rank.Joker, Suit.Hearts));
        // Spades = Black Joker
        _cards.Add(new Card(Rank.Joker, Suit.Spades));
    }

    /// <summary>
    /// Randomizes the order of the cards using the Fisher-Yates algorithm.
    /// </summary>
    public void Shuffle()
    {
        Random rng = new Random();
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = _cards[k];
            _cards[k] = _cards[n];
            _cards[n] = value;
        }
    }

    /// <summary>
    /// Removes and returns the top card from the deck.
    /// </summary>
    /// <returns>The top card, or null if empty.</returns>
    public Card DealCard()
    {
        if (_cards.Count == 0) return null;
        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
}