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

        public void InitializeGame()
        {
            var deck = CreateFullDeck();
            Shuffle(deck);

            // Deal 26 cards to each
            for (int i = 0; i < 26; i++)
            {
                PlayerDeck.Enqueue(deck[i]);
                ComputerDeck.Enqueue(deck[i + 26]);
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

        private void AwardLoot(Queue<Card> winnerDeck)
        {
            // 1. Loop through every card that was played in this round (or War)
            foreach (var card in LootPile)
            {
                // 2. Add it to the bottom of the winner's hand
                winnerDeck.Enqueue(card);
            }

            // 3. Clear the middle pile so it's empty for the next round
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
                AwardLoot(PlayerDeck);
                return "You won the WAR!";
            }
            else if (cFinal.Value > pFinal.Value)
            {
                AwardLoot(ComputerDeck);
                return "Computer won the WAR!";
            }
            else
            {
                // Double War! (Recursion)
                return "DOUBLE WAR! Draw again!";
            }
        }
    }
}