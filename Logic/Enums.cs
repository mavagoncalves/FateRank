namespace FateRank.Logic;

/// <summary>
/// Represents the four standard suits in a deck of cards.
/// </summary>
public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

/// <summary>
/// Represents the rank (value) of a playing card.
/// Values range from 2 to 14 (Ace), with Joker as the highest (15).
/// </summary>
public enum Rank
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14,
    Joker = 15
}