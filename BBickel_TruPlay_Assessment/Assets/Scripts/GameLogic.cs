using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static readonly int PlayerCount = 3;

    private DeckOfCards _deckOfCards;
    private PlayerHand[] _players;

    private void Awake(){
        _deckOfCards = new DeckOfCards();
        _deckOfCards.Shuffle();
        _deckOfCards.Print();

        _players = new PlayerHand[PlayerCount];
        for (int i = 0; i < PlayerCount; i++)
            _players[i] = new PlayerHand();
    }
    private void Start(){
        DealCards();
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
}
