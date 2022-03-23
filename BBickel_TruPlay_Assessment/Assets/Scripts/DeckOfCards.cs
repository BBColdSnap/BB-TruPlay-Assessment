using UnityEngine;

/// <summary>
/// DeckOfCards - Reference class for a deck of 52 unique cards (one of each suit+value). Handles shuffling and drawing.
/// </summary>
public class DeckOfCards{
    
    public static readonly int DeckSize = 52;           //How many total cards are in a deck
    public static readonly int SuitSize = 13;           //How many cards are in each suit
    
    private readonly Card[] _cardReferences;            //Array of all cards, regardless of if dealt out or not
    private int drawIndex = 0;                          //Index position of where to draw next

    /// <summary>
    /// Constructor
    /// </summary>
    public DeckOfCards(){
        _cardReferences = new Card[DeckSize];
        int index = 0;
        for(int suitIdx = 0; suitIdx<(int)Card.Suit.MAX; suitIdx++){
            for(int faceIdx = 0; faceIdx< SuitSize; faceIdx++, index++)
                _cardReferences[index] = new Card((Card.Suit)suitIdx, DetermineFaceValue(faceIdx), faceIdx);
        }
    }
    /// <summary>
    /// Shuffle all cards back into deck.
    /// </summary>
    public void Shuffle(){
        drawIndex = 0;
        for (int i=0; i< DeckSize; i++){
            for(int j=0; j<DeckSize; j++){
                if (i == j)
                    continue;

                int randIndex = Random.Range(0, DeckSize);
                Card temp = _cardReferences[randIndex];
                _cardReferences[randIndex] = _cardReferences[i];
                _cardReferences[i] = temp;
            }
        }
    }
    /// <summary>
    /// Draw cards from the top of the deck.
    /// </summary>
    /// <param name="drawCount">How many cards to try to draw</param>
    /// <returns>Array of 'Card' references. If less than 'drawCount' remaining, will draw all left.</returns>
    public Card[] DrawCards(int drawCount){
        int availableCount = Mathf.Min(drawCount, DeckSize - drawIndex);//Ensure we don't overdraw cards
        Card[] cards = new Card[availableCount];
        for (int i = 0; i < availableCount; i++)
            cards[i] = _cardReferences[drawIndex + i];
        drawIndex = availableCount;
        return cards;
    }
    /// <summary>
    /// Determine what the string value of the card is based on the index value.
    /// </summary>
    /// <param name="faceIndex">Index 0 to 'SuitSize'</param>
    /// <returns>String of the face value.</returns>
    private static string DetermineFaceValue(int faceIndex){
        if (faceIndex <= 7)
            return (faceIndex + 2).ToString();//Card numbers start at 2 so offset all numbers by 2 for display
        else if (faceIndex == 8)
            return "10";
        else if (faceIndex == 9)
            return "Jack";
        else if (faceIndex == 10)
            return "Queen";
        else if (faceIndex == 11)
            return "King";
        else
            return "Ace";
    }
#if UNITY_EDITOR
    /// <summary>
    /// (Editor Only) Build a string of this deck's values in current order.
    /// </summary>
    /// <returns>String containing formatted card values.</returns>
    public override string ToString(){
        System.Text.StringBuilder sb = new System.Text.StringBuilder("Deck:\n");
        for (int i = 0; i < DeckSize; i++)
            sb.Append(_cardReferences[i].ToString() + "\n");
        return sb.ToString();
    }
#endif
}
