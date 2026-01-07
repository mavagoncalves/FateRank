using System.Collections.Generic;

namespace FateRank.Logic;

public class GameEngine
{
    /// <summary>
    /// Handles all the game logic 
    /// </summary>
    public Player Human { get; private set; } = null!;
    public Player Computer { get; private set; } = null!;

    public int PlayerCardCount => Human?.GetCardCount() ?? 0;
    public int ComputerCardCount => Computer?.GetCardCount() ?? 0;

    /// <summary>
    /// Creates players, initializes and shuffles a standard deck, and deals half the cards to each player.
    /// </summary>
    public void InitializeGame()
    {
        Human = new Player("You");
        Computer = new Player("Computer");

        // 1. Create and Shuffle
        Deck deck = new Deck();
        deck.Initialize();
        deck.Shuffle();

        List<Card> allCards = deck.GetCards();

        // 2. Deal 26 cards to each
        List<Card> deck1 = new List<Card>();
        List<Card> deck2 = new List<Card>();

        for (int i = 0; i < 26; i++) deck1.Add(allCards[i]);
        for (int i = 26; i < 52; i++) deck2.Add(allCards[i]);

        Human.ReceiveCard(deck1);
        Computer.ReceiveCard(deck2);
    }

    /// <summary>
    /// Plays a single round by drawing a card from each player and comparing them.
    /// </summary>
    /// <param name="pCard">Outputs the player's drawn card for this round.</param>
    /// <param name="cCard">Outputs the computer's drawn card for this round.</param>
    /// <returns>A string describing the result: player win, computer win, "WAR!", or "GAME OVER" if cards are missing.</returns>
    public string PlayRound(out Card pCard, out Card cCard)
    {
        pCard = Human.PlayCard()!;
        cCard = Computer.PlayCard()!;

        // Handle empty hand / end of game safety
        if (pCard == null || cCard == null)
        {
            return "GAME OVER";
        }

        // 3. Use your Card class's built-in comparison
        int comparison = pCard.CompareTo(cCard);

        if (comparison > 0) // Player is higher
        {
            Human.ReceiveCard(new List<Card> { pCard, cCard });
            return "YOU WON THIS HAND!";
        }
        else if (comparison < 0) // Computer is higher
        {
            Computer.ReceiveCard(new List<Card> { cCard, pCard });
            return "COMPUTER WON THIS HAND";
        }
        else
        {
            return "WAR!";
        }
    }

    /// <summary>
    /// Executes the war sequence by placing stake cards into the provided pool, drawing battle cards,
    /// and awarding the pool to the winner. If a player is unable to draw required cards, forces game over.
    /// </summary>
    /// <param name="pool">A list used to accumulate the cards involved in the war (stakes and battle cards).</param>
    /// <param name="pWar">Outputs the player's final face-up war card (may be null if insufficient cards).</param>
    /// <param name="cWar">Outputs the computer's final face-up war card (may be null if insufficient cards).</param>
    /// <returns>A string indicating the war outcome, "WAR!" for another tie, or a game-over message if cards are insufficient.</returns>
    public string ExecuteWar(List<Card> pool, out Card pWar, out Card cWar)
    {
        // 1. Stake 3 hidden cards
        for (int i = 0; i < 3; i++)
        {
            Card? pBurn = Human.PlayCard();
            if (pBurn != null) pool.Add(pBurn);

            Card? cBurn = Computer.PlayCard();
            if (cBurn != null) pool.Add(cBurn);
        }

        // 2. Play Battle Cards
        pWar = Human.PlayCard()!;
        cWar = Computer.PlayCard()!;

        if (pWar == null || cWar == null)
        {
            // Recover cards if someone runs out mid-war
            if (pWar != null) pool.Add(pWar);
            if (cWar != null) pool.Add(cWar);
            
            ForceGameOver();
            return "GAME OVER (Not enough cards)";
        }

        pool.Add(pWar);
        pool.Add(cWar);

        // 3. Compare War Cards
        int comparison = pWar.CompareTo(cWar);

        if (comparison > 0)
        {
            Human.ReceiveCard(pool);
            return "YOU WON THE WAR!";
        }
        else if (comparison < 0)
        {
            Computer.ReceiveCard(pool);
            return "COMPUTER WON THE WAR";
        }
        else
        {
            return "WAR!";
        }
    }

    /// <summary>
    /// Forces the game to end by emptying the losing player's cards, based on current counts.
    /// This method repeatedly plays cards from the trailing player's hand until none remain.
    /// </summary>
    public void ForceGameOver()
    {
        if (Human.GetCardCount() > Computer.GetCardCount())
        {
            while(Computer.PlayCard() != null) { } 
        }
        else
        {
            while(Human.PlayCard() != null) { }
        }
    }
}