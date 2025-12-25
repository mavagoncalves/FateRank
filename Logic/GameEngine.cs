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
    }
}