using UnityEngine;
using System.Collections.Generic;

public class PlayerHand
{
    private List<Card> _playerCards;
    private List<Card> _playerWonCards;
    
    public PlayerHand(){
        _playerCards = new List<Card>();
        _playerWonCards = new List<Card>();
    }
    ~PlayerHand(){
        ClearAllCards();
    }
    public void ClearAllCards(){
        _playerCards.Clear();
        _playerWonCards.Clear();
    }
    public Card DrawTopCard(){
        if (_playerCards.Count == 0)
            ShuffleNewCardsIntoHand();
        Card topCard = _playerCards[0];
        _playerCards.RemoveAt(0);
        return topCard;
    }
    public void AddCardsToBottom(Card[] cards){
        _playerWonCards.AddRange(cards);
    }
    public int GetCardCount(){
        return _playerCards.Count + _playerWonCards.Count;
    }
    public void Print(){
#if UNITY_EDITOR
        Debug.Log(ToString());
#endif
    }
#if UNITY_EDITOR
    public override string ToString(){
        System.Text.StringBuilder sb = new System.Text.StringBuilder("PlayerHand:\n");
        for (int i = 0; i < _playerCards.Count; i++)
            sb.Append(string.Format("{0} of {1}\n", _playerCards[i].GetFaceValue(), _playerCards[i].GetSuit()));
        return sb.ToString();
    }
#endif
    private void ShuffleNewCardsIntoHand(){
        int count = _playerWonCards.Count;
        for (int i = 0; i < count; i++){
            for (int j = 0; j < count; j++){
                if (i == j)
                    continue;

                int randIndex = Random.Range(0, count);
                Card temp = _playerWonCards[randIndex];
                _playerWonCards[randIndex] = _playerWonCards[i];
                _playerWonCards[i] = temp;
            }
        }
        _playerCards.AddRange(_playerWonCards);
        _playerWonCards.Clear();
    }
}
