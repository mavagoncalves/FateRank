using System;
using System.Collections.Generic;

namespace FateRank.Logic;

public class Deck
{
    private List<Card> _cards = new List<Card>();

    /// <summary>
    /// Initializes the internal card list to a standard 52-card deck (Jokers excluded).
    /// </summary>
    public void Initialize()
    {
        _cards.Clear();

        // Iterate through the Enums to create the standard 52 cards
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                // Skip Joker for standard War
                if (rank == Rank.Joker) continue;

                // Uses your Card constructor: public Card(Rank rank, Suit suit)
                _cards.Add(new Card(rank, suit));
            }
        }
        //Adding jokers
        _cards.Add(new Card(Rank.Joker, Suit.Hearts));
        _cards.Add(new Card(Rank.Joker, Suit.Spades));
    }

    /// <summary>
    /// Randomly shuffles the deck in place using the Fisher–Yates algorithm.
    /// </summary>
    public void Shuffle()
    {
        Random rng = new Random();
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
        }
    }

    /// <summary>
    /// Returns the internal list of cards currently in the deck.
    /// </summary>
    /// <returns>The list of <see cref="Card"/> objects in the deck.</returns>
    public List<Card> GetCards()
    {
        return _cards;
    }
}