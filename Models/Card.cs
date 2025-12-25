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

        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
        }
    }
}