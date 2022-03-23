using UnityEngine;
using TMPro;

public class SceneCard : MonoBehaviour
{
    [SerializeField]
    private Renderer[] _suitRenderers;
    [SerializeField]
    private TextMeshPro[] _cardFaceValueTexts;
    [SerializeField]
    private Renderer _cardBackgroundRenderer;
    [SerializeField]
    private Texture2D _spadeTexture;
    [SerializeField]
    private Texture2D _heartTexture;
    [SerializeField]
    private Texture2D _clubTexture;
    [SerializeField]
    private Texture2D _diamondTexture;
    [SerializeField]
    private Color _blackSuitColor;
    [SerializeField]
    private Color _redSuitColor;
    [SerializeField]
    private Material _cardBackerMaterial;
    [SerializeField]
    private Material _cardDefaultMaterial;

    public void SetCardVisuals(Card cardData)
    {
        for (int i = 0; i < _cardFaceValueTexts.Length; i++)
            _cardFaceValueTexts[i].gameObject.SetActive(cardData != null);
        for (int i = 0; i < _suitRenderers.Length; i++)
            _suitRenderers[i].gameObject.SetActive(cardData != null);

        if (cardData == null)//Show just the card back
        {
            _cardBackgroundRenderer.material = _cardBackerMaterial;
            return;
        }
        
        _cardBackgroundRenderer.material = _cardDefaultMaterial;
        for (int i = 0; i < _cardFaceValueTexts.Length; i++)
        {
            _cardFaceValueTexts[i].text = cardData.GetFaceValue();
            _cardFaceValueTexts[i].color = DetermineSuitColor(cardData.GetSuit());

        }
        for (int i = 0; i < _suitRenderers.Length; i++)
            _suitRenderers[i].material.mainTexture = DetermineSuitTexture(cardData.GetSuit());
    }
    Texture DetermineSuitTexture(Card.Suit suit)
    {
        return suit switch
        {
            Card.Suit.SPADE => _spadeTexture,
            Card.Suit.HEART => _heartTexture,
            Card.Suit.CLUB => _clubTexture,
            Card.Suit.DIAMOND => _diamondTexture,
            _ => null,
        };
    }
    Color DetermineSuitColor(Card.Suit suit)
    {
        if (suit == Card.Suit.SPADE || suit == Card.Suit.CLUB)
            return _blackSuitColor;
        else
            return _redSuitColor;
    }
}
