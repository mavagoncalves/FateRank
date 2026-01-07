# FateRank - The War Card Game

FateRank is a cross-platform card game built with **.NET MAUI** and **C#**, featuring a Poker like UI, smooth animations, and a robust MVVM architecture. It implements the "War" card game rules with a full **54-card deck (including Jokers)**.

## Table of Contents
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [How to Play](#how-to-play)
- [Project Architecture](#project-architecture)
- [Asset Requirements](#asset-requirements)

---

## Features
* **Full 54-Card Deck:** Standard cards plus 2 Jokers (Red & Black).
* **Visual Feedback:** Dynamic UI updates for wins, losses, and "War" scenarios.
* **Polished UI:** Gold-rimmed card borders, semi-rounded aesthetics, and adaptive layouts.
* **MVVM Pattern:** Strict separation of Logic, UI (Views), and State (ViewModels).
* **Cross-Platform:** Runs on macOS (MacCatalyst), Windows, iOS, and Android.

---

## Prerequisites

To run this project, you need the following installed on your machine:

1.  **Code Editor:** [Visual Studio 2022](https://visualstudio.microsoft.com/) (Windows/Mac) or [VS Code](https://code.visualstudio.com/).
2.  **.NET 8.0 SDK:** [Download here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
3.  **MAUI Workloads:**
    Open your terminal or command prompt and run:
    ```bash
    dotnet workload install maui
    ```

---

## Installation & Setup

1.  **Clone the Repository**
    ```bash
    git clone [https://github.com/mavagoncalves/FateRank.git](https://github.com/mavagoncalves/FateRank.git)
    cd FateRank
    ```

2.  **Restore Dependencies**
    ```bash
    dotnet restore
    ```

3.  **Run the Game**
    
    * **On macOS:**
        ```bash
        dotnet build -t:Run -f net8.0-maccatalyst -p:_SuppressSdkCheck=true
        ```
    * **On Windows:**
        Select "Windows Machine" in Visual Studio and press **F5**.

---

## How to Play

The goal is to win all 54 cards in the deck.

1.  **The Deal:** The deck is divided evenly (27 cards each) between You and the Computer.
2.  **The Battle:** Click **DEAL** to play the top card from your deck.
    * **Higher Rank Wins:** The player with the higher card takes both cards.
    * **Ranks:** 2 (Lowest) → Ace → **Joker** (Highest).
3.  **WAR! (Tie):** If both cards have the same rank (e.g., two Kings):
    * **Stakes:** Each player puts 3 cards face-down ("staking").
    * **Battle:** A 4th card is revealed face-up.
    * **Winner:** The higher face-up card wins **all** cards in the pot (10 cards total!).
    * **Double War:** If the face-up cards match again, the war continues!
4.  **Victory:** The game ends when one player holds all 54 cards.

---

## Project Architecture

This project follows the **MVVM (Model-View-ViewModel)** design pattern.

### 1. Logic (Models)
* **Card.cs:** Defines the Card object (Rank, Suit, Image logic).
* **Deck.cs:** Manages deck creation (52 cards + 2 Jokers) and shuffling.
* **Player.cs:** Represents a player (Human/Computer) and their hand queue.
* **GameEngine.cs:** The "Brain". Handles rules, comparing cards, and executing wars.
* **Enums.cs:** Defines strict `Suit` (Hearts...) and `Rank` (Two...Ace, Joker).

### 2. ViewModels
* **MainViewModel.cs:** Connects the UI to the Game Engine. It handles data binding (`PlayerImage`, `Score`, `StatusText`) and commands (`DealCommand`).

### 3. Views
* **MainPage.xaml:** The visual layout (Grids, Borders, Images).
* **MainPage.xaml.cs:** minimal code-behind; simply initializes the Context.

---

## Asset Requirements

**Crucial:** The game relies on a specific naming convention for card images in `Resources/Images`.

Ensure your images are named exactly as follows (lowercase):

* **Format:** `card_[suit]_[rank].png`
* **Suits:** `hearts`, `diamonds`, `clubs`, `spades`
* **Ranks:** * Numbers: `02`, `03` ... `10` (Note the leading zero!)
    * Faces: `j` (Jack), `q` (Queen), `k` (King), `a` (Ace)
* **Jokers:**
    * `card_joker_red.png`
    * `card_joker_black.png`
* **Back:** `card_back.png`


---

**Enjoy FateRank!** 🃏