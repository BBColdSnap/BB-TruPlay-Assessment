using UnityEngine;
using System.Collections.Generic;

public class PlayerHand
{
    private List<Card> _playerCards;
    
    public PlayerHand(){
        _playerCards = new List<Card>();
    }
    ~PlayerHand(){
        _playerCards.Clear();
    }
    public void ClearAllCards(){
        _playerCards.Clear();
    }
    public Card DrawTopCard(){
        Card topCard = _playerCards[0];
        _playerCards.RemoveAt(0);
        return topCard;
    }
    public void AddCardsToBottom(Card[] cards){
        _playerCards.AddRange(cards);
    }
    public int GetCardCount(){
        return _playerCards.Count;
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
}
