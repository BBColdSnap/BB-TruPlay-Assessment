
/// <summary>
/// Card - Reference class for a single card. Has a display value, numeric comparison value, and a suit.
/// </summary>
public class Card{
    
    public enum Suit { SPADE=0, HEART, CLUB, DIAMOND, 
        MAX}                                            //Enum for available Card suits

    // Constant Members
    private readonly Suit _suit;                        //The suit of this card
    private readonly string _displayValue;              //The face value of this card (text)
    private readonly int _compareValue;                 //The value this card holds when comparing against others

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="suit">Card::Suit enum this card is in.</param>
    /// <param name="faceValue">Display string of this card.</param>
    /// <param name="compareValue">Comparison value of this card.</param>
    public Card(Suit suit, string faceValue, int compareValue){
        _suit = suit;
        _displayValue = faceValue;
        _compareValue = compareValue;
    }
    /// <summary>
    /// Get Card's Suit
    /// </summary>
    /// <returns>Card::Suit</returns>
    public Suit GetSuit(){
        return _suit;
    }
    /// <summary>
    /// Get Card's Face Value
    /// </summary>
    /// <returns>String face value</returns>
    public string GetFaceValue(){
        return _displayValue;
    }
    /// <summary>
    /// Get Card's Comparison Value
    /// </summary>
    /// <returns>Int 0-N of how high this card is.</returns>
    public int GetCompareValue(){
        return _compareValue;
    }
#if UNITY_EDITOR
    /// <summary>
    /// (Editor Only) Build a string of this card's values.
    /// </summary>
    /// <returns>String containing formatted card values.</returns>
    public override string ToString(){
        return string.Format("Card: {0} of {1}", _displayValue, _suit);
    }
#endif
}
