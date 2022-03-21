
public class Card
{
    public enum Suit { SPADE=0, HEART, CLUB, DIAMOND, MAX}

    private readonly Suit _suit;
    private readonly string _faceValue;

    public Card(Suit suit, string faceValue){
        _suit = suit;
        _faceValue = faceValue;
    }
    public Suit GetSuit(){
        return _suit;
    }
    public string GetFaceValue(){
        return _faceValue;
    }
}
