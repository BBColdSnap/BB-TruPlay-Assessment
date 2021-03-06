using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// PlayerHand - Reference of a player. Manage's the player's card pile(s).
/// </summary>
public class PlayerHand{

    //Private Members
    private List<Card> _playerCards;                    //Cards the player can draw from
    private List<Card> _playerWonCards;                 //Cards the player has won (will move to _playerCards if empty)
    private int _playerWonWars;                         //Tracker for how many wars this player has won.
    
    /// <summary>
    /// Public constructor
    /// </summary>
    public PlayerHand(){
        _playerCards = new List<Card>();
        _playerWonCards = new List<Card>();
    }
    /// <summary>
    /// Destructor
    /// </summary>
    ~PlayerHand(){
        ClearAllCards();
    }
    /// <summary>
    /// Clears all of the player's cards
    /// </summary>
    public void ClearAllCards(){
        _playerCards.Clear();
        _playerWonCards.Clear();
    }
    /// <summary>
    /// Pulls the top card from the player's pile.
    /// </summary>
    /// <returns>Card reference drawn.</returns>
    public Card DrawTopCard(){
        if (_playerCards.Count == 0)
            ShuffleNewCardsIntoHand();
        Card topCard = _playerCards[0];
        _playerCards.RemoveAt(0);
        return topCard;
    }
    /// <summary>
    /// Add new cards to the player's stack
    /// </summary>
    /// <param name="cards">Array of Card references to add</param>
    public void AddCardsToBottom(Card[] cards){
        _playerWonCards.AddRange(cards);
    }
    /// <summary>
    /// Get total number of player's remaining cards.
    /// </summary>
    /// <returns>Player's total remaining cards.</returns>
    public int GetTotalCardCount(){
        return _playerCards.Count + _playerWonCards.Count;
    }
    /// <summary>
    /// Get number of cards present in the player's 'draw' pile
    /// </summary>
    /// <returns></returns>
    public int GetDrawPileCount() {
        return _playerCards.Count;
    }
    /// <summary>
    /// Get number of cards present in the player's 'won' pile
    /// </summary>
    /// <returns></returns>
    public int GetWonPileCount() {
        return _playerWonCards.Count;
    }
    /// <summary>
    /// Get the total number of wars this player has won.
    /// </summary>
    /// <returns>Number of wars won.</returns>
    public int GetPlayerWarsWon() {
        return _playerWonWars;
    }
    /// <summary>
    /// Set the number of wars this player has won.
    /// </summary>
    /// <param name="warsWon">Number of wars won.</param>
    public void SetPlayerWarsWon(int warsWon) {
        _playerWonWars = warsWon;
    }
    /// <summary>
    /// (Editor Only) Print the user's card stack to the console
    /// </summary>
    public void Print(){
#if UNITY_EDITOR
        Debug.Log(ToString());
#endif
    }
#if UNITY_EDITOR
    /// <summary>
    /// (Editor Only) Build a string of all the player's cards.
    /// </summary>
    /// <returns>String containing formatted card values.</returns>
    public override string ToString(){
        System.Text.StringBuilder sb = new System.Text.StringBuilder("PlayerHand:\n");
        for (int i = 0; i < _playerCards.Count; i++)
            sb.Append(string.Format("{0} of {1}\n", _playerCards[i].GetFaceValue(), _playerCards[i].GetSuit()));
        for (int i = 0; i < _playerWonCards.Count; i++)
            sb.Append(string.Format("{0} of {1}\n", _playerWonCards[i].GetFaceValue(), _playerWonCards[i].GetSuit()));
        return sb.ToString();
    }
#endif
    /// <summary>
    /// Take all the player's won cards, shuffle them, and add them to the pool of available cards.
    /// </summary>
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
