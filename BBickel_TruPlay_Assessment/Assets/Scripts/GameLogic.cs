using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private DeckOfCards _deckOfCards;

    private void Awake(){
        _deckOfCards = new DeckOfCards();
        _deckOfCards.Shuffle();
        _deckOfCards.Print();
    }
}
