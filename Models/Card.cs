namespace FateRank.Models
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; }

        // This property creates the filename for your images automatically
        public string ImageSource => $"card_{Suit.ToLower()}_{Rank}.png";

        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
        }
    }
}