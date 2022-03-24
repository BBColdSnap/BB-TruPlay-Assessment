using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private AudioClip _cardDealAudioClip;               //Audio clip for when a card is dealt/lerped
    [SerializeField]
    private float _cardStackYBuffer = 0.0025f;          //Slight position adjustment to prevent Z fighting on stacked cards.
    
    //Private Members
    private PlayerHand[] _players;                      //References to game's players. Used to read card counts remaining.

    private GameObject _player1DrawObj_Large;           //Instantiated object of Player 1's draw pile (Large)
    private GameObject _player1DrawObj_Small;           //Instantiated object of Player 1's draw pile (Small)
    private GameObject _player1WonObj_Large;            //Instantiated object of Player 1's won pile (Large)
    private GameObject _player1WonObj_Small;            //Instantiated object of Player 1's won pile (Small)
    private SceneCard _player1CardObj;                  //Instantiated object of Player 1's active card (reused for dealing, copied for pool)

    private GameObject _player2DrawObj_Large;           //Instantiated object of Player 2's draw pile (Large)
    private GameObject _player2DrawObj_Small;           //Instantiated object of Player 2's draw pile (Small)
    private GameObject _player2WonObj_Large;            //Instantiated object of Player 2's won pile (Large)
    private GameObject _player2WonObj_Small;            //Instantiated object of Player 2's won pile (Small)
    private SceneCard _player2CardObj;                  //Instantiated object of Player 2's active card (reused for dealing, copied for pool)

    private AudioSource _audioSource;                   //Audio Source for playing Card sound effects;
    private List<GameObject> _cardPoolList;             //Card pool waiting to be lerped to a winning player.

    /// <summary>
    /// Initial reference creations
    /// </summary>
    private void Awake() {
        _cardPoolList = new List<GameObject>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _cardDealAudioClip;
    }
    /// <summary>
    /// Set references to the PlayerHands for this game. Performs Setup for player visual objects
    /// </summary>
    /// <param name="players">PlayerHand references</param>
    public void SetPlayerReferences(PlayerHand[] players){
        _players = players;

        //Player 1
        _player1DrawObj_Large = Instantiate(_largeDeckPrefab);
        _player1DrawObj_Large.transform.SetParent(_player1_DrawPile.transform);
        _player1DrawObj_Large.transform.localPosition = Vector3.zero;
        _player1DrawObj_Large.transform.localRotation = Quaternion.identity;
        _player1DrawObj_Large.transform.localScale = Vector3.one;

        _player1DrawObj_Small = Instantiate(_smallDeckPrefab);
        _player1DrawObj_Small.transform.SetParent(_player1_DrawPile.transform);
        _player1DrawObj_Small.transform.localPosition = Vector3.zero;
        _player1DrawObj_Small.transform.localRotation = Quaternion.identity;
        _player1DrawObj_Small.transform.localScale = Vector3.one;

        _player1WonObj_Large = Instantiate(_largeDiscardPrefab);
        _player1WonObj_Large.transform.SetParent(_player1_WonPile.transform);
        _player1WonObj_Large.transform.localPosition = Vector3.zero;
        _player1WonObj_Large.transform.localRotation = Quaternion.identity;
        _player1WonObj_Large.transform.localScale = Vector3.one;

        _player1WonObj_Small = Instantiate(_smallDiscardPrefab);
        _player1WonObj_Small.transform.SetParent(_player1_WonPile.transform);
        _player1WonObj_Small.transform.localPosition = Vector3.zero;
        _player1WonObj_Small.transform.localRotation = Quaternion.identity;
        _player1WonObj_Small.transform.localScale = Vector3.one;

        GameObject player1SuitCardObj = Instantiate(_cardPrefab);
        player1SuitCardObj.transform.SetParent(_player1_TurnCard.transform);
        player1SuitCardObj.transform.localPosition = Vector3.zero;
        player1SuitCardObj.transform.localRotation = Quaternion.identity;
        player1SuitCardObj.transform.localScale = Vector3.one;
        player1SuitCardObj.SetActive(false);
        _player1CardObj = player1SuitCardObj.GetComponent<SceneCard>();

        //Player 2
        _player2DrawObj_Large = Instantiate(_largeDeckPrefab);
        _player2DrawObj_Large.transform.SetParent(_player2_DrawPile.transform);
        _player2DrawObj_Large.transform.localPosition = Vector3.zero;
        _player2DrawObj_Large.transform.localRotation = Quaternion.identity;
        _player2DrawObj_Large.transform.localScale = Vector3.one;

        _player2DrawObj_Small = Instantiate(_smallDeckPrefab);
        _player2DrawObj_Small.transform.SetParent(_player2_DrawPile.transform);
        _player2DrawObj_Small.transform.localPosition = Vector3.zero;
        _player2DrawObj_Small.transform.localRotation = Quaternion.identity;
        _player2DrawObj_Small.transform.localScale = Vector3.one;

        _player2WonObj_Large = Instantiate(_largeDiscardPrefab);
        _player2WonObj_Large.transform.SetParent(_player2_WonPile.transform);
        _player2WonObj_Large.transform.localPosition = Vector3.zero;
        _player2WonObj_Large.transform.localRotation = Quaternion.identity;
        _player2WonObj_Large.transform.localScale = Vector3.one;

        _player2WonObj_Small = Instantiate(_smallDiscardPrefab);
        _player2WonObj_Small.transform.SetParent(_player2_WonPile.transform);
        _player2WonObj_Small.transform.localPosition = Vector3.zero;
        _player2WonObj_Small.transform.localRotation = Quaternion.identity;
        _player2WonObj_Small.transform.localScale = Vector3.one;

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
        //Draw Piles
        int player1DrawCardCount = _players[0].GetDrawPileCount();
        int player2DrawCardCount = _players[1].GetDrawPileCount();
        _player1DrawObj_Large.SetActive(player1DrawCardCount > _playerSmallPileThreshold);
        _player1DrawObj_Small.SetActive(player1DrawCardCount > 0 && player1DrawCardCount <= _playerSmallPileThreshold);

        _player2DrawObj_Large.SetActive(player2DrawCardCount > _playerSmallPileThreshold);
        _player2DrawObj_Small.SetActive(player2DrawCardCount > 0 && player2DrawCardCount <= _playerSmallPileThreshold);

        //Won Piles
        int player1WonCardCount = _players[0].GetWonPileCount();
        int player2WonCardCount = _players[1].GetWonPileCount();
        _player1WonObj_Large.SetActive(player1WonCardCount > _playerSmallPileThreshold);
        _player1WonObj_Small.SetActive(player1WonCardCount > 0 && player1WonCardCount <= _playerSmallPileThreshold);

        _player2WonObj_Large.SetActive(player2WonCardCount > _playerSmallPileThreshold);
        _player2WonObj_Small.SetActive(player2WonCardCount > 0 && player2WonCardCount <= _playerSmallPileThreshold);
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

        _audioSource.Play();

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

        CopyObjectToPool(_player1CardObj.gameObject);
        CopyObjectToPool(_player2CardObj.gameObject);
        _player1CardObj.gameObject.SetActive(false);
        _player2CardObj.gameObject.SetActive(false);

        if (winnerIndex != -1){//Only lerp to a pile if there's not a War
            Transform winnerTransform = (winnerIndex == 0) ? _player1WonObj_Large.transform : _player2WonObj_Large.transform;
            int cardLerpsRunning = _cardPoolList.Count;
            for(int i=0; i<_cardPoolList.Count; i++) {
                GameObject lerpCardObj = _cardPoolList[i];
                StartCoroutine(LerpCardObjectToTransform(lerpCardObj.transform, winnerTransform, () => {
                    lerpCardObj.SetActive(false);
                    Destroy(lerpCardObj);
                    cardLerpsRunning--;
                }));
            }
            _cardPoolList.Clear();

            while (cardLerpsRunning > 0)
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
            _audioSource.Play();

            int cardLerpsRunning = 0;
            if(player1CardCount > 0){
                player1CardCount--;
                cardLerpsRunning++;
                _player1CardObj.transform.position = _player1_DrawPile.transform.position;
                _player1CardObj.transform.rotation = _player1_DrawPile.transform.rotation;
                StartCoroutine(LerpCardObjectToTransform(_player1CardObj.transform, _player1_TurnCard.transform, () => {
                    CopyObjectToPool(_player1CardObj.gameObject);
                    cardLerpsRunning--;
                }));
            }
            if(player2CardCount > 0){
                player2CardCount--;
                cardLerpsRunning++;
                _player2CardObj.transform.position = _player2_DrawPile.transform.position;
                _player2CardObj.transform.rotation = _player2_DrawPile.transform.rotation;
                StartCoroutine(LerpCardObjectToTransform(_player2CardObj.transform, _player2_TurnCard.transform, () => {
                    CopyObjectToPool(_player2CardObj.gameObject);
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
    /// Lerps a card from current position, rotation, and scale to match that of 'targetTransform'
    /// </summary>
    /// <param name="cardTransform">The transform to lerp</param>
    /// <param name="targetTransform">The transform to use as end point.</param>
    /// <param name="completedCallback">Callback for when lerp is complete</param>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    private IEnumerator LerpCardObjectToTransform(Transform cardTransform, Transform targetTransform, Action completedCallback = null){
        Vector3 startPos = cardTransform.position;
        Quaternion startRot = cardTransform.rotation;
        Vector3 startScale = cardTransform.localScale;

        float yValue = DetermineCardPoolYMax(targetTransform.position.y);
        Vector3 targetPosition = (Vector3.right * targetTransform.position.x) + (Vector3.up * yValue) + (Vector3.forward * targetTransform.position.z);

        float startTime = Time.time;
        float endTime = startTime + _cardPlayLerpTime;
        while(Time.time < endTime){
            float percent = (Time.time - startTime) / _cardPlayLerpTime;
            cardTransform.position = Vector3.Lerp(startPos, targetPosition, percent);
            cardTransform.rotation = Quaternion.Lerp(startRot, targetTransform.rotation, percent);
            cardTransform.localScale = Vector3.Lerp(startScale, targetTransform.localScale, percent);
            yield return null;
        }
        
        cardTransform.position = targetPosition;
        cardTransform.rotation = targetTransform.rotation;
        cardTransform.localScale = targetTransform.localScale;
        completedCallback?.Invoke();
    }
    /// <summary>
    /// Copy the given card object into the card pool to keep it visually present.
    /// </summary>
    /// <param name="cardObject">Card object to copy, at same position and rotation</param>
    private void CopyObjectToPool(GameObject cardObject) {
        GameObject cardCopyObject = Instantiate(cardObject);
        cardCopyObject.transform.SetParent(cardObject.transform.parent);
        cardCopyObject.transform.SetPositionAndRotation(cardObject.transform.position, cardObject.transform.rotation);
        cardCopyObject.transform.localScale = cardObject.transform.localScale;
        _cardPoolList.Add(cardCopyObject);
    }
    /// <summary>
    /// Checks all the cards in the object pool for the maximum Y position. Used to ensure cards stack properly
    /// </summary>
    /// <param name="defaultY"></param>
    /// <returns></returns>
    private float DetermineCardPoolYMax(float defaultY) {
        float maxY = defaultY;
        for(int i=0; i<_cardPoolList.Count; i++) {
            if (_cardPoolList[i].transform.position.y >= maxY)
                maxY = _cardPoolList[i].transform.position.y + _cardStackYBuffer;
        }
        return maxY;
    }
}
