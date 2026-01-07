using System;
using System.Collections.Generic;
using System.Linq;
using FateRank.Models;

namespace FateRank.Logic
{
    /// <summary>
    /// This class handles all the logic of the cards and gameplay like
    ///  dealing shuffling, executing a war, etc..
    /// <summary>
    public class GameEngine
    {
        public Queue<Card> PlayerDeck { get; set; } = new Queue<Card>();
        public Queue<Card> ComputerDeck { get; set; } = new Queue<Card>();
        public List<Card> LootPile { get; set; } = new List<Card>();
        public int PlayerCardCount => PlayerDeck.Count + PlayerWinPile.Count;
        public int ComputerCardCount => ComputerDeck.Count + ComputerWinPile.Count;
        public List<Card> PlayerWinPile { get; set; } = new List<Card>();
        public List<Card> ComputerWinPile { get; set; } = new List<Card>();

        /// <summary>
        /// Initializes the game by creating a full deck, shuffling it,
        /// and dealing half the cards to the player and the computer.
        /// </summary>
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


        /// <summary>
        /// Creates a full standard deck of cards and returns it as a list.
        /// Includes all suits and ranks, plus two jokers. Each card has a numeric
        /// Value property used for comparisons during gameplay.
        /// </summary>
        /// <returns>A list containing all cards in the deck.</returns>
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


        /// <summary>
        /// Randomly shuffles the provided list of cards in place using the Fisher–Yates algorithm.
        /// </summary>
        /// <param name="list">The list of cards to shuffle.</param>
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


        /// <summary>
        /// Plays a single round by drawing the top card from each deck, placing them into the loot pile,
        /// and determining the round outcome (player win, computer win, or war).
        /// </summary>
        /// <param name="pCard">Outputs the players card for this round.</param>
        /// <param name="cCard">Outputs the computers card for this round.</param>
        /// <returns>A string indicating the round result: a win message, "WAR!", or "Game Over!".</returns>
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


        /// <summary>
        /// Transfers all cards currently in the loot pile to the specified win pile and clears the loot pile.
        /// </summary>
        /// <param name="targetWinPile">The win pile that will receive the loot.</param>
        private void AwardLoot(List<Card> targetWinPile)
        {
            foreach (var card in LootPile)
            {
                targetWinPile.Add(card);
            }
            LootPile.Clear();
        }


        /// <summary>
        /// Performs the 'war' sequence when two cards tie: ensures decks are refilled from win piles if needed,
        /// places the required face-down and face-up cards into the loot pile, and determines the war outcome.
        /// </summary>
        /// <param name="pFinal">Outputs the player's final face-up card used to decide the war (may be null if insufficient cards).</param>
        /// <param name="cFinal">Outputs the computer's final face-up card used to decide the war (may be null if insufficient cards).</param>
        /// <returns>A string indicating the war outcome, or a not-enough-cards message when a player cannot continue.</returns>
        public string ExecuteWar(out Card pFinal, out Card cFinal)
        {
            // Check if we need to refill  for WAR from win pile
            // If deck has < 4 cards BUT we have a win pile, we must refill NOW.
            if (PlayerDeck.Count < 4 && PlayerWinPile.Count > 0)
            {
                Shuffle(PlayerWinPile);
                foreach (var c in PlayerWinPile) PlayerDeck.Enqueue(c);
                PlayerWinPile.Clear();
            }
            
            if (ComputerDeck.Count < 4 && ComputerWinPile.Count > 0)
            {
                Shuffle(ComputerWinPile);
                foreach (var c in ComputerWinPile) ComputerDeck.Enqueue(c);
                ComputerWinPile.Clear();
            }

            // check if players are TRULY out of cards
            if (PlayerDeck.Count < 4)
            {
                pFinal = null; cFinal = null;
                // Force count to 0 to ensure the UI knows the game is over
                PlayerDeck.Clear(); 
                PlayerWinPile.Clear();
                return "Not enough cards! YOU LOSE.";
            }
            if (ComputerDeck.Count < 4)
            {
                pFinal = null; cFinal = null;
                ComputerDeck.Clear();
                ComputerWinPile.Clear();
                return "Not enough cards! CPU LOSES.";
            }

            // continue war logic (draw 3 face down)
            for (int i = 0; i < 3; i++)
            {
                LootPile.Add(PlayerDeck.Dequeue());
                LootPile.Add(ComputerDeck.Dequeue());
            }

            // draw the deciding card (Face up)
            pFinal = PlayerDeck.Dequeue();
            cFinal = ComputerDeck.Dequeue();
            LootPile.Add(pFinal);
            LootPile.Add(cFinal);

            // determine Winner
            if (pFinal.Value > cFinal.Value)
            {
                AwardLoot(PlayerWinPile);
                return "YOU WON THE WAR!";
            }
            else if (cFinal.Value > pFinal.Value)
            {
                AwardLoot(ComputerWinPile);
                return "CPU WON THE WAR!";
            }
            else
            {
                return "WAR!"; //double war
            }
        }


        /// <summary>
        /// If the provided deck is empty and the win pile contains cards, shuffles the win pile
        /// and moves its cards back into the deck, then clears the win pile.
        /// </summary>
        /// <param name="deck">The primary deck to refill.</param>
        /// <param name="winPile">The win pile to draw cards from when refilling.</param>
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