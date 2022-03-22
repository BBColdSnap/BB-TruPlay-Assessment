using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    private const int PlayerCount = 2;
    private const int WarCardTargetCount = 3;

    [SerializeField]
    private float _turnDelay = 1f;

    private DeckOfCards _deckOfCards;
    private PlayerHand[] _players;
    private List<Card> _turnCardPot;

    private void Awake(){
        _deckOfCards = new DeckOfCards();
        _players = new PlayerHand[PlayerCount];
        for (int i = 0; i < PlayerCount; i++)
            _players[i] = new PlayerHand();
        _turnCardPot = new List<Card>();
    }
    private void Start(){
        DealCards();
        StartCoroutine(RunTurnsOnDelay());
    }
    private IEnumerator RunTurnsOnDelay(){
        
        while(true){
            RunTurn();
            bool gameOver = CheckForGameOver();
            if (gameOver == true){
                Start();//Endless play to test for devious shuffle
                yield break;
            }

            float delayEnd = Time.time + _turnDelay;
            while (Time.time < delayEnd)
                yield return null;
        }
    }
    private void DealCards(){
        for (int i = 0; i < PlayerCount; i++)
            _players[i].ClearAllCards();

        _deckOfCards.Shuffle();

        int minHandSize = DeckOfCards.DeckSize / PlayerCount;
        int remainder = DeckOfCards.DeckSize % PlayerCount;

        for(int i=0; i<PlayerCount; i++){
            int playerHandSize = minHandSize;
            if (remainder > 0){//If we have extra cards, spread them out between players
                playerHandSize++;
                remainder--;
            }

            _players[i].AddCardsToBottom(_deckOfCards.DrawCards(playerHandSize));
            _players[i].Print();
        }
    }
    private void RunTurn(){

        int maxCardValue = int.MinValue;
        int maxCardPlayer = -1;
        int warPlayer = -1;
        bool triggerWar = false;
        
        for (int i = 0; i < PlayerCount; i++){
            Card playerCard = _players[i].DrawTopCard();
            _turnCardPot.Add(playerCard);

            Debug.Log(string.Format("Player {0} plays {1}", i, playerCard.ToString()));

            if (maxCardValue < playerCard.GetCompareValue())
            {
                maxCardValue = playerCard.GetCompareValue();
                maxCardPlayer = i;
            }
            else if (maxCardValue == playerCard.GetCompareValue())
            {
                triggerWar = true;
                warPlayer = i;//TODO: Expand logic to factor in several players
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
    private bool CheckForIncompleteWar(int warPlayer1, int warPlayer2, ref int winningPlayerIndex)
    {
        /*
          * If one player has at least 4 cards (3 for war, 1 to compare), and the other has less, first player wins
          * If both players do not have enough (only counts in 3+ player games), player with more cards wins
          * Otherwise, proceed
        */
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
    private bool CheckForGameOver(){
        for (int i = 0; i < PlayerCount; i++){
            if(_players[i].GetCardCount() == DeckOfCards.DeckSize){
                Debug.LogWarning(string.Format("Player {0} Wins!", (i + 1)));
                return true;
            }
        }
        return false;
    }
}
