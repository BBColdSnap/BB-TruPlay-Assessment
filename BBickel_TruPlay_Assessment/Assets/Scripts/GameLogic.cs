using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GameLogic - Core class for turn ordering and player instantiation and management
/// </summary>
public class GameLogic : MonoBehaviour{
    private const int PlayerCount = 2;              //How many players are in this game
    private const int WarCardTargetCount = 3;       //How many cards are used in a "War" pile

    [SerializeField]
    private float _turnDelay = 1f;                  //How long each turn will wait before automatically proceeding

    private DeckOfCards _deckOfCards;               //Reference to this game's Card deck
    private PlayerHand[] _players;                  //Array of players in this game (size 'PlayerCount')
    private List<Card> _turnCardPot;                //List of Card references that holds the pot for each turn
    private int _gamesCompleted = 0;                //Internal tracker for how many games are finished during a session.

    /// <summary>
    /// Initial reference creations
    /// </summary>
    private void Awake(){
        _deckOfCards = new DeckOfCards();
        _players = new PlayerHand[PlayerCount];
        for (int i = 0; i < PlayerCount; i++)
            _players[i] = new PlayerHand();
        _turnCardPot = new List<Card>();
    }
    /// <summary>
    /// Gameplay setup
    /// </summary>
    private void Start(){
        DealCards();
        StartCoroutine(RunTurnsOnDelay());
    }
    /// <summary>
    /// Automate turns for now, no user action required
    /// </summary>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    private IEnumerator RunTurnsOnDelay(){
        
        while(true){
            RunTurn();
            bool gameOver = CheckForGameOver();
            if (gameOver == true){
                yield break;
            }

            float delayEnd = Time.time + _turnDelay;
            while (Time.time < delayEnd)
                yield return null;
        }
    }
    /// <summary>
    /// Deal out the entire deck of cards among all players
    /// </summary>
    private void DealCards(){
        for (int i = 0; i < PlayerCount; i++)
            _players[i].ClearAllCards();

        _deckOfCards.Shuffle();

        int minHandSize = DeckOfCards.DeckSize / PlayerCount;
        int remainder = DeckOfCards.DeckSize % PlayerCount;

        for(int i=0; i<PlayerCount; i++){
            int playerHandSize = minHandSize;
            if (remainder > 0){                     //If we have extra cards, spread them out
                playerHandSize++;
                remainder--;
            }

            _players[i].AddCardsToBottom(_deckOfCards.DrawCards(playerHandSize));
            _players[i].Print();
        }
    }
    /// <summary>
    /// Run a single turn. Compare cards, and go to War if applicable.
    /// </summary>
    private void RunTurn(){
        int maxCardValue = int.MinValue;
        int maxCardPlayer = -1;
        int warPlayer = -1;
        bool triggerWar = false;
        
        for (int i = 0; i < PlayerCount; i++){
            Card playerCard = _players[i].DrawTopCard();
            _turnCardPot.Add(playerCard);

            Debug.Log(string.Format("Player {0} plays {1}", i, playerCard.ToString()));

            if (maxCardValue < playerCard.GetCompareValue()){
                maxCardValue = playerCard.GetCompareValue();
                maxCardPlayer = i;
            }
            else if (maxCardValue == playerCard.GetCompareValue()){
                triggerWar = true;
                warPlayer = i;                      //TODO: Expand logic to factor in several players
            }
        }

        if(triggerWar == false){
            _players[maxCardPlayer].AddCardsToBottom(_turnCardPot.ToArray());
            _turnCardPot.Clear();
            Debug.Log("Player " + maxCardPlayer + " takes pot - "+ _players[maxCardPlayer].GetCardCount());
        }
        else{
            RunWar(maxCardPlayer, warPlayer);
        }
    }
    /// <summary>
    /// Run the "War" logic, deal out 3 cards then return to Turn to compare last card. 
    /// Handles lose-case if player does not have enough cards to fight.
    /// </summary>
    /// <param name="warPlayer1">Index of player entering War (1 of 2)</param>
    /// <param name="warPlayer2">Index of player entering War (2 of 2)</param>
    private void RunWar(int warPlayer1, int warPlayer2){
        Debug.Log("WAR!");
        int playerAutoWinIndex = -1;
        bool playerWillAutoWin = CheckForIncompleteWar(warPlayer1, warPlayer2, ref playerAutoWinIndex);
        if (playerWillAutoWin == true){

            int losingPlayerIndex = (playerAutoWinIndex == warPlayer1) ? warPlayer2 : warPlayer1;
            while (_players[losingPlayerIndex].GetCardCount() > 0)
                _turnCardPot.Add(_players[losingPlayerIndex].DrawTopCard());
            _players[playerAutoWinIndex].AddCardsToBottom(_turnCardPot.ToArray());
            _turnCardPot.Clear();
            Debug.Log("Player " + playerAutoWinIndex + " takes pot War (early)- " + _players[playerAutoWinIndex].GetCardCount());
            return;
        }

        for (int i = 0; i < PlayerCount; i++){
            for (int j = 0; j < WarCardTargetCount; j++){
                Card playerCard = _players[i].DrawTopCard();
                _turnCardPot.Add(playerCard);
            }
        }
        
        RunTurn();
    }
    /// <summary>
    /// Helper function used to check if a player will automatically lose if going to war due to insufficient card count (Needs 3 for the war, and 1 to compare).
    /// </summary>
    /// <param name="warPlayer1">Index of player entering War (1 of 2)</param>
    /// <param name="warPlayer2">Index of player entering War (2 of 2)</param>
    /// <param name="winningPlayerIndex">Reference parameter, filled in with index of winning player if applicable.</param>
    /// <returns>True, if War is auto-won. False if War will proceed as normal.</returns>
    private bool CheckForIncompleteWar(int warPlayer1, int warPlayer2, ref int winningPlayerIndex){
        int player1CardCount = _players[warPlayer1].GetCardCount();
        int player2CardCount = _players[warPlayer2].GetCardCount();
        const int minCardCount = WarCardTargetCount + 1;
        if(player1CardCount < minCardCount && player2CardCount > player1CardCount){
            winningPlayerIndex = warPlayer2;
            return true;
        }
        else if(player1CardCount > player2CardCount && player2CardCount < minCardCount){
            winningPlayerIndex = warPlayer1;
            return true;
        }
        else{
            return false;
        }
    }
    /// <summary>
    /// Checks if any player has collected all the cards.
    /// </summary>
    /// <returns>True if a player has all the cards.</returns>
    private bool CheckForGameOver(){
        for (int i = 0; i < PlayerCount; i++){
            if(_players[i].GetCardCount() == DeckOfCards.DeckSize){
                Debug.LogWarning(string.Format("Player {0} Wins! Games Complete: {1}", (i + 1), ++_gamesCompleted));
                return true;
            }
        }
        return false;
    }
}
