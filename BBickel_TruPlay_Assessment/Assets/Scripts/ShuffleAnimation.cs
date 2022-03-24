using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ShuffleAnimation - Very basic Game intro animation showing shuffling cards in stages
/// </summary>
public class ShuffleAnimation : MonoBehaviour {

    //Inspector Fields
    [SerializeField]
    private GameObject[] _shuffleStageObjects;
    [SerializeField]
    private float _shuffleTransitionTime = 0.5f;

    /// <summary>
    /// Runs the Shuffle Animation.
    /// </summary>
    /// <param name="callback">Callback when all animations are done.</param>
    /// <returns>IEnumerator. Use StartCoroutine</returns>
    public IEnumerator PlayShuffleAnimation(Action callback = null) {
        for (int i = 0; i < _shuffleStageObjects.Length; i++)
            _shuffleStageObjects[i].SetActive(false);

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
