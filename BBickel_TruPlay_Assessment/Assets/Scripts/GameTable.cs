using System;
using System.Collections;
using UnityEngine;

public class GameTable : MonoBehaviour
{
    private const int playerSmallPileThreshold = 10;

    [SerializeField]
    private GameObject _player1_DrawPile;
    [SerializeField]   
    private GameObject _player1_WonPile;
    [SerializeField]   
    private GameObject _player1_TurnCard;
                       
    [SerializeField]   
    private GameObject _player2_DrawPile;
    [SerializeField]   
    private GameObject _player2_WonPile;
    [SerializeField]   
    private GameObject _player2_TurnCard;
                       
    [SerializeField]   
    private GameObject _largeDeckPrefab;
    [SerializeField]   
    private GameObject _smallDeckPrefab;
    [SerializeField]   
    private GameObject _largeDiscardPrefab;
    [SerializeField]   
    private GameObject _smallDiscardPrefab;
    [SerializeField]   
    private GameObject _cardPrefab;

    [SerializeField]
    private float _cardPlayLerpTime = 0.5f;

    private PlayerHand[] _players;
    private GameObject _player1DrawObj;
    private GameObject _player1WonObj;
    private SceneCard _player1CardObj;

    private GameObject _player2DrawObj;
    private GameObject _player2WonObj;
    private SceneCard _player2CardObj;

    public void SetPlayerReferences(PlayerHand[] players)
    {
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
    public void UpdateCardVisuals()
    {
        //Check player draw piles and won piles sizes

        //Update Text displays
    }
    public IEnumerator ShowPlayerCards(Card player1Card, Card player2Card, int winnerIndex, Action completedCallback = null)
    {
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

        if(winnerIndex != -1)//Only lerp to a pile if there's not a War
        {
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
    public IEnumerator ShowPlayerWar(int player1CardCount, int player2CardCount, Action completedCallback = null)
    {
        _player1CardObj.gameObject.SetActive(true);
        _player1CardObj.SetCardVisuals(null);
        _player2CardObj.gameObject.SetActive(true);
        _player2CardObj.SetCardVisuals(null);

        //Lerp 3 cards to position (pile below display)
        while (player1CardCount > 0 || player2CardCount > 0)
        {
            int cardLerpsRunning = 0;
            if(player1CardCount > 0)
            {
                player1CardCount--;
                cardLerpsRunning++;
                _player1CardObj.transform.position = _player1_DrawPile.transform.position;
                _player1CardObj.transform.rotation = _player1_DrawPile.transform.rotation;
                StartCoroutine(LerpCardObjectToTransform(_player1CardObj.transform, _player1_TurnCard.transform, () => {
                    cardLerpsRunning--;
                }));
            }
            if(player2CardCount > 0)
            {
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
    public void RunShuffleAnimation()
    {

    }
    IEnumerator LerpCardObjectToTransform(Transform cardTransform, Transform targetTransform, Action completedCallback = null)
    {
        Vector3 startPos = cardTransform.position;
        Quaternion startRot = cardTransform.rotation;

        float startTime = Time.time;
        float endTime = startTime + _cardPlayLerpTime;
        while(Time.time < endTime)
        {
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
