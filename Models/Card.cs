namespace FateRank.Models
{
    /// <summary>
    /// Represents a single playing card with a <see cref="Suit"/>, <see cref="Rank"/>, and a numeric <see cref="Value"/>.
    /// The <see cref="ImageSource"/> property returns the UI image filename for the card.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Gets or sets the suit of the card like "spades", "hearts", etc...
        /// </summary>
        public string Suit { get; set; } 

        /// <summary>
        /// Gets or sets the rank of the card  like "2", "jack", "ace".
        /// </summary>
        public string Rank { get; set; } 

        /// <summary>
        /// Gets or sets the numeric value used for comparisons during gameplay.
        /// </summary>
        public int Value { get; set; }   

        /// <summary>
        /// Gets the image file name corresponding to this card, computed from its suit and rank.
        /// Jokers are handled as special cases (<c>card_joker_black.png</c>).
        /// </summary>
        /// <returns>The image file name to use for UI display.</returns>
        public string ImageSource 
        {
            get 
            {
                // Special case for Jokers
                if (Rank.ToLower() == "joker")
                {
                    return $"card_joker_{Suit.ToLower()}.png"; // e.g., card_joker_black.png
                }

                string displayRank = Rank.ToLower();
                if (int.TryParse(Rank, out int rankNum) && rankNum < 10)
                {
                    displayRank = rankNum.ToString("D2"); 
                }
                else if (displayRank == "jack") displayRank = "j";
                else if (displayRank == "queen") displayRank = "q";
                else if (displayRank == "king") displayRank = "k";
                else if (displayRank == "ace") displayRank = "a";

                return $"card_{Suit.ToLower()}_{displayRank}.png";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class with the specified suit, rank, and value.
        /// </summary>
        /// <param name="suit">The card's suit</param>
        /// <param name="rank">The card's rank</param>
        /// <param name="value">The numeric value used for gameplay comparisons.</param>
        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
        }
    }
}