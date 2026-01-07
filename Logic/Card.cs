using System;

namespace FateRank.Logic;

/// <summary>
/// Represents a single playing card using strict Enums for type safety.
/// Implements IComparable to allow easy sorting and comparison.
/// </summary>
public class Card : IComparable<Card>
{
    /// <summary>
    /// Gets the rank (value) of the card (e.g., Ace, King, Joker).
    /// </summary>
    public Rank Rank { get; }

    /// <summary>
    /// Gets the suit of the card. For Jokers, this determines color (Hearts/Diamonds = Red).
    /// </summary>
    public Suit Suit { get; }

    /// <summary>
    /// Generates the UI image filename based on rank and suit.
    /// Handles standard cards (02-10, j, q, k, a) and Jokers (red/black).
    /// </summary>
    public string ImageSource
    {
        get
        {
            // --- JOKERS ---
            if (Rank == Rank.Joker)
            {
                // Map suits to colors: Hearts/Diamonds -> Red, Spades/Clubs -> Black
                string color = (Suit == Suit.Hearts || Suit == Suit.Diamonds) ? "red" : "black";
                return $"card_joker_{color}.png";
            }

            // --- HANDLE STANDARD CARDS ---
            int rankValue = (int)Rank;
            string rankSuffix;

            if (rankValue < 10)
            {
                rankSuffix = rankValue.ToString("D2");
            }
            else if (rankValue == 10)
            {
                rankSuffix = "10";
            }
            else
            {
                rankSuffix = Rank switch
                {
                    Rank.Jack => "j",
                    Rank.Queen => "q",
                    Rank.King => "k",
                    Rank.Ace => "a",
                    _ => rankValue.ToString()
                };
            }

            return $"card_{Suit.ToString().ToLower()}_{rankSuffix}.png";
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Card"/> class.
    /// </summary>
    /// <param name="rank">The rank of the card.</param>
    /// <param name="suit">The suit of the card.</param>
    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }

    /// <summary>
    /// Compares this card to another card based on Rank.
    /// </summary>
    public int CompareTo(Card other)
    {
        if (other == null) return 1;
        return Rank.CompareTo(other.Rank);
    }
}