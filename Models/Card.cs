namespace FateRank.Models
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; }

        // This method creates the filename for your images automatically
        public string ImageSource 
        {
            get 
            {
                string displayRank = Rank;

                // Handle the leading zero for numbers 2-9
                if (int.TryParse(Rank, out int rankNum) && rankNum < 10)
                {
                    displayRank = rankNum.ToString("D2"); // Turns "2" into "02"
                }
                // Handle face cards to match your "Q", "K", "A" format
                else if (Rank.ToLower() == "jack") displayRank = "J";
                else if (Rank.ToLower() == "queen") displayRank = "Q";
                else if (Rank.ToLower() == "king") displayRank = "K";
                else if (Rank.ToLower() == "ace") displayRank = "A";

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