using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ShuffleAnimation - Very basic Game intro animation showing shuffling cards in stages
/// </summary>
public class ShuffleAnimation : MonoBehaviour {

    //Inspector Fields
    [SerializeField]
    private GameObject[] _shuffleStageObjects;          //Objects to cycle through to shuffle
    [SerializeField]
    private float _shuffleTransitionTime = 0.5f;        //Time to show each object in cycle
    [SerializeField]
    private AudioClip _bridgeSoundEffect;               //Card Bridging sound effect

    //Private Members
    private AudioSource _audioSource;                   //Audio source on object to play audio

    /// <summary>
    /// Initial reference creations
    /// </summary>
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Runs the Shuffle Animation.
    /// </summary>
    /// <param name="callback">Callback when all animations are done.</param>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    public IEnumerator PlayShuffleAnimation(Action callback = null) {
        for (int i = 0; i < _shuffleStageObjects.Length; i++)
            _shuffleStageObjects[i].SetActive(false);

        _audioSource.clip = _bridgeSoundEffect;
        _audioSource.Play();
        int stageIndex = 0;
        while(stageIndex < _shuffleStageObjects.Length) {
            _shuffleStageObjects[stageIndex].SetActive(true);
            float proceedTime = Time.time + _shuffleTransitionTime;
            while (Time.time < proceedTime)
                yield return null;
            _shuffleStageObjects[stageIndex].SetActive(false);
            stageIndex++;
        }
        callback?.Invoke();
    }
}
