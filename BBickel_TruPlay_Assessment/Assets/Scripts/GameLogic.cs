using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    public static readonly int PlayerCount = 2;
    private static readonly int WarCardTargetCount = 3;

    [SerializeField]
    private float _turnDelay = 1f;

    private DeckOfCards _deckOfCards;
    private PlayerHand[] _players;
    private List<Card> _turnCardPot;

    private void Awake(){
        _deckOfCards = new DeckOfCards();
        _deckOfCards.Shuffle();
        Debug.Log(_deckOfCards.ToString());

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
            if (gameOver == true)
                yield break;

            float delayEnd = Time.time + _turnDelay;
            while (Time.time < delayEnd)
                yield return null;
        }
    }
    private void DealCards(){
        for (int i = 0; i < PlayerCount; i++)
            _players[i].ClearAllCards();

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
                warPlayer = i;
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

        /*
           * If one player has at least 4 cards (3 for war, 1 to compare), and the other has less, first player wins
           * If both players do not have enough (only counts in 3+ player games), player with more cards wins
           * Otherwise, proceed
       */
        bool playerWillAutoWin = true;
        int winningPlayer = -1;
        for (int i = 0; i < PlayerCount; i++){
            if (winningPlayer == -1 && _players[i].GetCardCount() >= WarCardTargetCount + 1){
                winningPlayer = i;
            }
            else if (_players[i].GetCardCount() >= WarCardTargetCount + 1){
                winningPlayer = -1;
                playerWillAutoWin = false;//Multiple players have enough cards, proceed as normal
            }
        }

        if(playerWillAutoWin == true){
            int otherPlayer = (winningPlayer == warPlayer1) ? warPlayer1 : warPlayer2;
            while (_players[otherPlayer].GetCardCount() > 0)
                _turnCardPot.Add(_players[otherPlayer].DrawTopCard());
            _players[winningPlayer].AddCardsToBottom(_turnCardPot.ToArray());
            _turnCardPot.Clear();
            Debug.Log("Player " + winningPlayer + " takes pot War (early)- " + _players[winningPlayer].GetCardCount());
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
    private bool CheckForGameOver(){
        for (int i = 0; i < PlayerCount; i++){
            if(_players[i].GetCardCount() == DeckOfCards.DeckSize){
                Debug.Log(string.Format("Player {0} Wins!", (i + 1)));
                return true;
            }
        }
        return false;
    }
}
