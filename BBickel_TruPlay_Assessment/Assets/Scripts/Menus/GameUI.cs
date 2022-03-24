using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// GameUI - UI for displaying player cards counts, and back button
/// </summary>
public class GameUI : MonoBehaviour
{
    //Public Types
    public delegate void GameUIEvent();                 //Public type to create callbacks

    //Publicly Accessed Members
    public GameUIEvent PlayButtonPressed;               //Subscribable Event for when we

    //Inspector Fields
    [SerializeField]
    private string _menuSceneName;                      //Scene to load for sameplay
    [SerializeField]
    private TextMeshProUGUI _player1CardCountLabel;     //Scene to load for sameplay
    [SerializeField]
    private TextMeshProUGUI _player1WonCountLabel;      //Scene to load for sameplay
    [SerializeField]
    private TextMeshProUGUI _player2CardCountLabel;     //Scene to load for sameplay
    [SerializeField]
    private TextMeshProUGUI _player2WonCountLabel;      //Scene to load for sameplay
    [SerializeField]
    private GameObject _playButtonObject;               //UI Play Button. Shows and hides contextually.
    [SerializeField]
    private Slider _timeScaleSlider;                    //UI Slider for time scale adjustment
    [SerializeField]
    private float _timeScaleMin = 1f;                   //Minimum Time scale value
    [SerializeField]
    private float _timeScaleMax = 100f;                 //Maximum Time scale value

    /// <summary>
    /// Set up default menu state.
    /// </summary>
    private void Start() {
        _playButtonObject.SetActive(false);
        _timeScaleSlider.value = Mathf.InverseLerp(_timeScaleMin, _timeScaleMax, Time.timeScale);
    }
    /// <summary>
    /// BackButton callback connected to prefab. Loads the _menuSceneName Scene
    /// </summary>
    public void BackButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_menuSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    /// <summary>
    /// PlayButton callback connected to prefab. Calls the PlayButtonPressed event;
    /// </summary>
    public void PlayButton() {
        PlayButtonPressed?.Invoke();
        _playButtonObject.SetActive(false);
    }
    /// <summary>
    /// Public method to indicate a game as completed
    /// </summary>
    public void GameOver() {
        _playButtonObject.SetActive(true);
    }
    /// <summary>
    /// Public method to update the card count labels for both players.
    /// </summary>
    /// <param name="player1">Player 1 Reference</param>
    /// <param name="player2">Player 2 Reference</param>
    public void UpdatePlayerLabels(PlayerHand player1, PlayerHand player2) {
        _player1CardCountLabel.text = player1.GetDrawPileCount().ToString();
        _player1WonCountLabel.text = player1.GetWonPileCount().ToString();
        _player2CardCountLabel.text = player2.GetDrawPileCount().ToString();
        _player2WonCountLabel.text = player2.GetWonPileCount().ToString();
    }
    /// <summary>
    /// Callback for Slider UI control
    /// </summary>
    public void TimeSliderMoved() {
        Time.timeScale = Mathf.Lerp(_timeScaleMin, _timeScaleMax, _timeScaleSlider.value);
    }
}
