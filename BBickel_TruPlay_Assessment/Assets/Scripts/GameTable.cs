using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// GameTable - Graphical components of War game, referenced on Table prefab
/// </summary>
public class GameTable : MonoBehaviour{
    
    // Constant Members
    private const int _playerSmallPileThreshold = 10;   //Threshold of cards to determine 'large' versus 'small' deck or won piles

    //Inspector Fields
    [SerializeField]
    private GameObject _player1_DrawPile;               //Scene reference of Player 1's draw pile position transform
    [SerializeField]   
    private GameObject _player1_WonPile;                //Scene reference of Player 1's won pile position transform
    [SerializeField]   
    private GameObject _player1_TurnCard;               //Scene reference of Player 1's played card end position transform

    [SerializeField]   
    private GameObject _player2_DrawPile;               //Scene reference of Player 2's draw pile position transform
    [SerializeField]   
    private GameObject _player2_WonPile;                //Scene reference of Player 2's won pile position transform
    [SerializeField]   
    private GameObject _player2_TurnCard;               //Scene reference of Player 2's played card end position transform
                       
    [SerializeField]   
    private GameObject _largeDeckPrefab;                //Prefab refernce of Large player draw deck
    [SerializeField]   
    private GameObject _smallDeckPrefab;                //Prefab reference of Small player draw deck
    [SerializeField]   
    private GameObject _largeDiscardPrefab;             //Prefab reference of Large player won pile
    [SerializeField]   
    private GameObject _smallDiscardPrefab;             //Prefab reference of Small player won pile
    [SerializeField]   
    private GameObject _cardPrefab;                     //Prefab reference of generic card object to use as cards are played

    [SerializeField]
    private float _cardPlayLerpTime = 0.5f;             //Duration for each card to lerp from Draw pile to the end play position

    //Private Members
    private PlayerHand[] _players;                      //References to game's players. Used to read card counts remaining.

    private GameObject _player1DrawObj;                 //Instantiated object of Player 1's draw pile
    private GameObject _player1WonObj;                  //Instantiated object of Player 1's won pile
    private SceneCard _player1CardObj;                  //Instantiated object of Player 1's active card (reused for all play)

    private GameObject _player2DrawObj;                 //Instantiated object of Player 2's draw pile
    private GameObject _player2WonObj;                  //Instantiated object of Player 2's won pile
    private SceneCard _player2CardObj;                  //Instantiated object of Player 2's active card (reused for all play)

    /// <summary>
    /// Set references to the PlayerHands for this game. Performs Setup for player visual objects
    /// </summary>
    /// <param name="players">PlayerHand references</param>
    public void SetPlayerReferences(PlayerHand[] players){
        _players = players;

        //Player 1
        _player1DrawObj = Instantiate(_largeDeckPrefab);
        _player1DrawObj.transform.SetParent(_player1_DrawPile.transform);
        _player1DrawObj.transform.localPosition = Vector3.zero;
        _player1DrawObj.transform.localRotation = Quaternion.identity;
        _player1DrawObj.transform.localScale = Vector3.one;

        _player1WonObj = Instantiate(_largeDiscardPrefab);
        _player1WonObj.transform.SetParent(_player1_WonPile.transform);
        _player1WonObj.transform.localPosition = Vector3.zero;
        _player1WonObj.transform.localRotation = Quaternion.identity;
        _player1WonObj.transform.localScale = Vector3.one;

        GameObject player1SuitCardObj = Instantiate(_cardPrefab);
        player1SuitCardObj.transform.SetParent(_player1_TurnCard.transform);
        player1SuitCardObj.transform.localPosition = Vector3.zero;
        player1SuitCardObj.transform.localRotation = Quaternion.identity;
        player1SuitCardObj.transform.localScale = Vector3.one;
        player1SuitCardObj.SetActive(false);
        _player1CardObj = player1SuitCardObj.GetComponent<SceneCard>();

        //Player 2
        _player2DrawObj = Instantiate(_largeDeckPrefab);
        _player2DrawObj.transform.SetParent(_player2_DrawPile.transform);
        _player2DrawObj.transform.localPosition = Vector3.zero;
        _player2DrawObj.transform.localRotation = Quaternion.identity;
        _player2DrawObj.transform.localScale = Vector3.one;
               
        _player2WonObj = Instantiate(_largeDiscardPrefab);
        _player2WonObj.transform.SetParent(_player2_WonPile.transform);
        _player2WonObj.transform.localPosition = Vector3.zero;
        _player2WonObj.transform.localRotation = Quaternion.identity;
        _player2WonObj.transform.localScale = Vector3.one;

        GameObject player2SuitCardObj = Instantiate(_cardPrefab);
        player2SuitCardObj.transform.SetParent(_player2_TurnCard.transform);
        player2SuitCardObj.transform.localPosition = Vector3.zero;
        player2SuitCardObj.transform.localRotation = Quaternion.identity;
        player2SuitCardObj.transform.localScale = Vector3.one;
        player2SuitCardObj.SetActive(false);
        _player2CardObj = player2SuitCardObj.GetComponent<SceneCard>();
    }
    /// <summary>
    /// Updates the state of the player objects to reflect current card counts.
    /// </summary>
    public void UpdateCardVisuals(){
        //Check player draw piles and won piles sizes

        //Update Text displays
    }
    /// <summary>
    /// Lerps player cards from respective Draw piles to center of table, then lerps to player's pot for player 'winnerIndex'
    /// </summary>
    /// <param name="player1Card">Reference to Player 1's card</param>
    /// <param name="player2Card">Reference to Player 2's card</param>
    /// <param name="winnerIndex">Index of player that wins cards. -1 if no winner</param>
    /// <param name="completedCallback">Callback for when all lerps are complete</param>
    /// <returns>IEnumerator. Use StartCoroutine.</returns>
    public IEnumerator ShowPlayerCards(Card player1Card, Card player2Card, int winnerIndex, Action completedCallback = null){
        //Update card displays
        _player1CardObj.gameObject.SetActive(true);
        _player1CardObj.SetCardVisuals(player1Card);
        _player2CardObj.gameObject.SetActive(true);
        _player2CardObj.SetCardVisuals(player2Card);

        //Lerp cards to position
        int cardLerpsComplete = 0;
        _player1CardObj.transform.position = _player1_DrawPile.transform.position;
        _player1CardObj.transform.rotation = _player1_DrawPile.transform.rotation;
        StartCoroutine(LerpCardObjectToTransform(_player1CardObj.transform, _player1_TurnCard.transform, ()=> { 
            cardLerpsComplete++; 
        }));

        _player2CardObj.transform.position = _player2_DrawPile.transform.position;
        _player2CardObj.transform.rotation = _player2_DrawPile.transform.rotation;
        StartCoroutine(LerpCardObjectToTransform(_player2CardObj.transform, _player2_TurnCard.transform, () => { 
            cardLerpsComplete++; 
        }));

        while (cardLerpsComplete < 2)
            yield return null;

        //Pause
        float pauseTime = Time.time + 1f;
        while (Time.time < pauseTime)
            yield return null;

        if(winnerIndex != -1){//Only lerp to a pile if there's not a War
            //Lerp cards to winner's pile
            Transform winnerTransform = (winnerIndex == 0) ? _player1WonObj.transform : _player2WonObj.transform;
            cardLerpsComplete = 0;
            StartCoroutine(LerpCardObjectToTransform(_player1CardObj.transform, winnerTransform, () => {
                _player1CardObj.gameObject.SetActive(false);
                cardLerpsComplete++;
            }));
            StartCoroutine(LerpCardObjectToTransform(_player2CardObj.transform, winnerTransform, () => {
                _player2CardObj.gameObject.SetActive(false);
                cardLerpsComplete++;
            }));

            while (cardLerpsComplete < 2)
                yield return null;
        }
       
        completedCallback?.Invoke();
    }
    /// <summary>
    /// Lerps player cards from respective Draw piles to center of table, for showing players going to War.
    /// </summary>
    /// <param name="player1CardCount">How many cards Player 1 contributes to War</param>
    /// <param name="player2CardCount">How many cards Player 2 contributes to War</param>
    /// <param name="completedCallback">Callback for when all lerps are complete</param>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    public IEnumerator ShowPlayerWar(int player1CardCount, int player2CardCount, Action completedCallback = null){
        _player1CardObj.gameObject.SetActive(true);
        _player1CardObj.SetCardVisuals(null);
        _player2CardObj.gameObject.SetActive(true);
        _player2CardObj.SetCardVisuals(null);

        //Lerp 3 cards to position (pile below display)
        while (player1CardCount > 0 || player2CardCount > 0){
            int cardLerpsRunning = 0;
            if(player1CardCount > 0){
                player1CardCount--;
                cardLerpsRunning++;
                _player1CardObj.transform.position = _player1_DrawPile.transform.position;
                _player1CardObj.transform.rotation = _player1_DrawPile.transform.rotation;
                StartCoroutine(LerpCardObjectToTransform(_player1CardObj.transform, _player1_TurnCard.transform, () => {
                    cardLerpsRunning--;
                }));
            }
            if(player2CardCount > 0){
                player2CardCount--;
                cardLerpsRunning++;
                _player2CardObj.transform.position = _player2_DrawPile.transform.position;
                _player2CardObj.transform.rotation = _player2_DrawPile.transform.rotation;
                StartCoroutine(LerpCardObjectToTransform(_player2CardObj.transform, _player2_TurnCard.transform, () => {
                    cardLerpsRunning--;
                }));
            }

            while (cardLerpsRunning > 0)
                yield return null;
        }

        _player1CardObj.gameObject.SetActive(false);
        _player2CardObj.gameObject.SetActive(false);
        completedCallback?.Invoke();
    }
    /// <summary>
    /// Runs Game Start shuffle animation
    /// </summary>
    /// <param name="completedCallback">Callback for when all animations are complete</param>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    public IEnumerator RunShuffleAnimation(Action completedCallback = null){
        completedCallback?.Invoke();
        yield break;
    }
    /// <summary>
    /// Lerps a card from current position and rotation to match that of 'targetTransform'
    /// </summary>
    /// <param name="cardTransform">The transform to lerp</param>
    /// <param name="targetTransform">The transform to use as end point.</param>
    /// <param name="completedCallback">Callback for when lerp is complete</param>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    private IEnumerator LerpCardObjectToTransform(Transform cardTransform, Transform targetTransform, Action completedCallback = null){
        Vector3 startPos = cardTransform.position;
        Quaternion startRot = cardTransform.rotation;

        float startTime = Time.time;
        float endTime = startTime + _cardPlayLerpTime;
        while(Time.time < endTime){
            float percent = (Time.time - startTime) / _cardPlayLerpTime;
            cardTransform.position = Vector3.Lerp(startPos, targetTransform.position, percent);
            cardTransform.rotation = Quaternion.Lerp(startRot, targetTransform.rotation, percent);
            yield return null;
        }
        cardTransform.position = targetTransform.position;
        cardTransform.rotation = targetTransform.rotation;
        completedCallback?.Invoke();
    }
}
