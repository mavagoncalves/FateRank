using System;
using System.Collections.Generic;

namespace FateRank.Logic;

public class Deck
{
    private List<Card> _cards = new List<Card>();

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
    }

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

    public List<Card> GetCards()
    {
        return _cards;
    }
}