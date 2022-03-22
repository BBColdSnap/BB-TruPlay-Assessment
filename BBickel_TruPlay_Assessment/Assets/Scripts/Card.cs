
public class Card
{
    public enum Suit { SPADE=0, HEART, CLUB, DIAMOND, MAX}

    private readonly Suit _suit;
    private readonly string _displayValue;
    private readonly int _compareValue;

    public Card(Suit suit, string faceValue, int compareValue){
        _suit = suit;
        _displayValue = faceValue;
        _compareValue = compareValue;
    }
    public Suit GetSuit(){
        return _suit;
    }
    public string GetFaceValue(){
        return _displayValue;
    }
    public int GetCompareValue(){
        return _compareValue;
    }
#if UNITY_EDITOR
    public override string ToString(){
        return string.Format("Card: {0} of {1}", _displayValue, _suit);
    }
#endif
}
