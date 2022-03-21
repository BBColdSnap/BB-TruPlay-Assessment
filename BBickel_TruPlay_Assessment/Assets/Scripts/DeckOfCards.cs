using UnityEngine;

public class DeckOfCards
{
    public static readonly int DeckSize = 52;
    public static readonly int SuitSize = 13;
    
    private readonly Card[] _cardReferences;
    private int drawIndex = 0;

    public DeckOfCards(){
        _cardReferences = new Card[DeckSize];
        int index = 0;
        for(int suitIdx = 0; suitIdx<(int)Card.Suit.MAX; suitIdx++){
            for(int faceIdx = 0; faceIdx< SuitSize; faceIdx++, index++)
                _cardReferences[index] = new Card((Card.Suit)suitIdx, DetermineFaceValue(faceIdx));//Card numbers start at 2
        }
    }
    public void Shuffle(){
        for(int i=0; i< DeckSize; i++){
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
    public Card[] DrawCards(int drawCount){
        int availableCount = Mathf.Min(drawCount, DeckSize - drawIndex);//Ensure we don't overdraw cards
        Card[] cards = new Card[availableCount];
        for (int i = 0; i < availableCount; i++)
            cards[i] = _cardReferences[drawIndex + i];
        drawIndex = availableCount;
        return cards;
    }
    public void Print(){
        Debug.Log(ToString());
    }
    public override string ToString() { 
        System.Text.StringBuilder sb = new System.Text.StringBuilder("Deck:\n");
        for (int i = 0; i < DeckSize; i++)
            sb.Append(string.Format("{0} of {1}\n", _cardReferences[i].GetFaceValue(), _cardReferences[i].GetSuit()));
        return sb.ToString();
    }
    private static string DetermineFaceValue(int faceIndex){
        if (faceIndex <= 7)
            return (faceIndex + 2).ToString();
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
}
