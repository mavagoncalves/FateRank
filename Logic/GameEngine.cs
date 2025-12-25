using System;
using System.Collections.Generic;
using System.Linq;
using FateRank.Models;

namespace FateRank.Logic
{
    public class GameEngine
    {
        public Queue<Card> PlayerDeck { get; set; } = new Queue<Card>();
        public Queue<Card> ComputerDeck { get; set; } = new Queue<Card>();
        public List<Card> LootPile { get; set; } = new List<Card>();
        public int PlayerCardCount => PlayerDeck.Count + PlayerWinPile.Count;
        public int ComputerCardCount => ComputerDeck.Count + ComputerWinPile.Count;
        public List<Card> PlayerWinPile { get; set; } = new List<Card>();
        public List<Card> ComputerWinPile { get; set; } = new List<Card>();

        public void InitializeGame()
        {
            var deck = CreateFullDeck();
            Shuffle(deck);

            // Deal 26 cards to each
            for (int i = 0; i < 27; i++)
            {
                PlayerDeck.Enqueue(deck[i]);
                ComputerDeck.Enqueue(deck[i + 27]);
            }
        }


        private List<Card> CreateFullDeck()
        {
            string[] suits = { "spades", "hearts", "diamonds", "clubs" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king", "ace" };
            var newDeck = new List<Card>();

            foreach (var suit in suits)
            {
                for (int i = 0; i < ranks.Length; i++)
                {
                    newDeck.Add(new Card(suit, ranks[i], i + 2));
                }
            }

            newDeck.Add(new Card("black", "joker", 15));
            newDeck.Add(new Card("red", "joker", 15));

            return newDeck;
        }


        private void Shuffle(List<Card> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public string PlayRound(out Card pCard, out Card cCard)
        {
            // 1. Move won cards back to the main deck if empty
            RefillDeckIfEmpty(PlayerDeck, PlayerWinPile);
            RefillDeckIfEmpty(ComputerDeck, ComputerWinPile);

            // 2. Check for Game Over
            if (PlayerDeck.Count == 0 || ComputerDeck.Count == 0)
            {
                pCard = null; cCard = null;
                return "Game Over!";
            }

            pCard = PlayerDeck.Dequeue();
            cCard = ComputerDeck.Dequeue();
            LootPile.Add(pCard);
            LootPile.Add(cCard);

            if (pCard.Value > cCard.Value)
            {
                AwardLoot(PlayerWinPile); // Give to WIN PILE
                return "You win the round!";
            }
            else if (cCard.Value > pCard.Value)
            {
                AwardLoot(ComputerWinPile); // Give to WIN PILE
                return "Computer wins the round!";
            }
            
            return "WAR!";
        }


        private void AwardLoot(List<Card> targetWinPile)
        {
            foreach (var card in LootPile)
            {
                targetWinPile.Add(card);
            }
            LootPile.Clear();
        }


        public string ExecuteWar(out Card pFinal, out Card cFinal)
        {
            // Check if players have enough cards to go to war (at least 4 cards)
            if (PlayerDeck.Count < 4 || ComputerDeck.Count < 4)
            {
                pFinal = null; cFinal = null;
                return "Not enough cards for WAR! Game Over.";
            }

            // Draw 3 cards each and put them in the LootPile (face down)
            for (int i = 0; i < 3; i++)
            {
                LootPile.Add(PlayerDeck.Dequeue());
                LootPile.Add(ComputerDeck.Dequeue());
            }

            // Draw the 4th card (face up) to decide the winner
            pFinal = PlayerDeck.Dequeue();
            cFinal = ComputerDeck.Dequeue();
            LootPile.Add(pFinal);
            LootPile.Add(cFinal);

            if (pFinal.Value > cFinal.Value)
            {
                AwardLoot(PlayerWinPile);
                return "You won the WAR!";
            }
            else if (cFinal.Value > pFinal.Value)
            {
                AwardLoot(ComputerWinPile);
                return "Computer won the WAR!";
            }
            else
            {
                // Double War! (Recursion)
                return "DOUBLE WAR! Draw again!";
            }
        }


        private void RefillDeckIfEmpty(Queue<Card> deck, List<Card> winPile)
        {
            if (deck.Count == 0 && winPile.Count > 0)
            {
                Shuffle(winPile); // Existing shuffle method
                
                foreach (var card in winPile)
                {
                    deck.Enqueue(card); // Move from list to queue
                }
                
                winPile.Clear(); // Empty the win pile
            }
        }
    }
}