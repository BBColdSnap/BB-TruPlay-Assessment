using UnityEngine;
using TMPro;

/// <summary>
/// SceneCard - Graphical representation of a 'Card' object.
/// </summary>
public class SceneCard : MonoBehaviour{
    
    //Inspector Fields
    [SerializeField]
    private Renderer[] _suitRenderers;                  //Renderers on object that should receive the Suit emblem Texture
    [SerializeField]
    private TextMeshPro[] _cardFaceValueTexts;          //TextMeshPro reference to display the card's face value
    [SerializeField]
    private Renderer _cardBackgroundRenderer;           //Renderer to swap for _cardDefaultMaterial or _cardBackerMaterial based on face up or down
    [SerializeField]
    private Texture2D _spadeTexture;                    //Texture to use for 'Spade' suit    
    [SerializeField]
    private Texture2D _heartTexture;                    //Texture to use for 'Heart' suit
    [SerializeField]
    private Texture2D _clubTexture;                     //Texture to use for 'Club' suit
    [SerializeField]
    private Texture2D _diamondTexture;                  //Texture to use for 'Diamond' suit
    [SerializeField]
    private Color _blackSuitColor;                      //Color to apply for 'Spade' or 'Club' text
    [SerializeField]
    private Color _redSuitColor;                        //Color to apply for 'Heart' or 'Diamond' text
    [SerializeField]
    private Material _cardBackerMaterial;               //Material to use for card if "face down"
    [SerializeField]
    private Material _cardDefaultMaterial;              //Material to use for card if "face up"

    /// <summary>
    /// Sets up the 3D card's visual elements based on the provided 'Card' reference
    /// </summary>
    /// <param name="cardData"></param>
    public void SetCardVisuals(Card cardData){
        for (int i = 0; i < _cardFaceValueTexts.Length; i++)
            _cardFaceValueTexts[i].gameObject.SetActive(cardData != null);
        for (int i = 0; i < _suitRenderers.Length; i++)
            _suitRenderers[i].gameObject.SetActive(cardData != null);

        if (cardData == null){//Show just the card back
            _cardBackgroundRenderer.material = _cardBackerMaterial;
            return;
        }
        
        _cardBackgroundRenderer.material = _cardDefaultMaterial;
        for (int i = 0; i < _cardFaceValueTexts.Length; i++){
            _cardFaceValueTexts[i].text = cardData.GetFaceValue();
            _cardFaceValueTexts[i].color = DetermineSuitColor(cardData.GetSuit());
        }
        for (int i = 0; i < _suitRenderers.Length; i++)
            _suitRenderers[i].material.mainTexture = DetermineSuitTexture(cardData.GetSuit());
    }
    /// <summary>
    /// Helper function to determine which Texture to use for a given 'Card.Suit'
    /// </summary>
    /// <param name="suit">The card's suit</param>
    /// <returns>Member Texture reference matching the suit.</returns>
    private Texture DetermineSuitTexture(Card.Suit suit){
        return suit switch{
            Card.Suit.SPADE => _spadeTexture,
            Card.Suit.HEART => _heartTexture,
            Card.Suit.CLUB => _clubTexture,
            Card.Suit.DIAMOND => _diamondTexture,
            _ => null,
        };
    }
    /// <summary>
    /// Helper function to determine which color to use for a given 'Card.Suit'
    /// </summary>
    /// <param name="suit">The card's suit</param>
    /// <returns>Color for the card to use</returns>
    private Color DetermineSuitColor(Card.Suit suit){
        if (suit == Card.Suit.SPADE || suit == Card.Suit.CLUB)
            return _blackSuitColor;
        else
            return _redSuitColor;
    }
}
