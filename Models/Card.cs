namespace FateRank.Models
{
    public class Card
    {
        public string Suit { get; set; } 
        public string Rank { get; set; } 
        public int Value { get; set; }   

        public string ImageSource 
        {
            get 
            {
                string displayRank = Rank;

                // Handle numbers 2-9 to add the "0" (e.g., "02")
                if (int.TryParse(Rank, out int rankNum) && rankNum < 10)
                {
                    displayRank = rankNum.ToString("D2"); 
                }
                // Handle face cards to match your uppercase filenames (Q, K, A, J)
                else if (Rank.ToLower() == "jack") displayRank = "J";
                else if (Rank.ToLower() == "queen") displayRank = "Q";
                else if (Rank.ToLower() == "king") displayRank = "K";
                else if (Rank.ToLower() == "ace") displayRank = "A";
                else if (Rank == "10") displayRank = "10";

                // This must match "card_suit_RANK.png"
                // Using .ToUpper() for displayRank to match your "Q" and "J" files
                return $"card_{Suit.ToLower()}_{displayRank.ToUpper()}.png";
            }
        }

        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
        }
    }
}