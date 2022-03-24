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

    //Private Constant Values
    private readonly string 
        _warLabelFormatText = "{0} Wars Won";           //Dynamic format text for player war labels

    //Inspector Fields
    [SerializeField]
    private string _menuSceneName;                      //Scene to load for sameplay
    [SerializeField]
    private TextMeshProUGUI _player1CardCountLabel;     //Cards remaining in Player 1's Draw pile
    [SerializeField]
    private TextMeshProUGUI _player1WonCountLabel;      //Cards in Player 1's Won pile
    [SerializeField]
    private TextMeshProUGUI _player1WarCountLabel;      //Wars Player 1 has won
    [SerializeField]
    private TextMeshProUGUI _player2CardCountLabel;     //Cards remaining in Player 2's Draw pile
    [SerializeField]
    private TextMeshProUGUI _player2WonCountLabel;      //Cards in Player 2's Won pile
    [SerializeField]
    private TextMeshProUGUI _player2WarCountLabel;      //Wars Player 2 has won
    [SerializeField]
    private GameObject _playButtonObject;               //UI Play Button. Shows and hides contextually.
    [SerializeField]
    private GameObject _playerWinLabel;                 //UI Text to display when the player wins
    [SerializeField]
    private GameObject _playerLoseLabel;                //UI Text to display when the player loses
    [SerializeField]
    private Slider _timeScaleSlider;                    //UI Slider for time scale adjustment
    [SerializeField]
    private float _timeScaleMin = 1f;                   //Minimum Time scale value
    [SerializeField]
    private float _timeScaleMax = 100f;                 //Maximum Time scale value
    [SerializeField]
    private AudioClip _gameWonAudio;                    //Player Won audio clip
    [SerializeField]
    private AudioClip _gameLostAudio;                   //Player Lost audio clip

    //Private Members
    private AudioSource _audioSource;                   //Audio source on object to play audio

    /// <summary>
    /// Initial reference creations
    /// </summary>
    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Set up default menu state.
    /// </summary>
    private void Start() {
        _playButtonObject.SetActive(false);
        _playerWinLabel.gameObject.SetActive(false);
        _playerLoseLabel.gameObject.SetActive(false);
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
        _playerWinLabel.gameObject.SetActive(false);
        _playerLoseLabel.gameObject.SetActive(false);
    }
    /// <summary>
    /// Public method to indicate a game as completed
    /// </summary>
    public void GameOver(bool playerWon) {
        AudioClip gameOverClip = (playerWon) ? _gameWonAudio : _gameLostAudio;
        _audioSource.clip = gameOverClip;
        _audioSource.Play();
        _playButtonObject.SetActive(true);
        _playerWinLabel.gameObject.SetActive(playerWon);
        _playerLoseLabel.gameObject.SetActive(!playerWon);
    }
    /// <summary>
    /// Public method to update the labels for both players.
    /// </summary>
    /// <param name="player1">Player 1 Reference</param>
    /// <param name="player2">Player 2 Reference</param>
    public void UpdatePlayerLabels(PlayerHand player1, PlayerHand player2) {
        _player1CardCountLabel.text = player1.GetDrawPileCount().ToString();
        _player1WonCountLabel.text = player1.GetWonPileCount().ToString();
        _player1WarCountLabel.text = string.Format(_warLabelFormatText, player1.GetPlayerWarsWon());

        _player2CardCountLabel.text = player2.GetDrawPileCount().ToString();
        _player2WonCountLabel.text = player2.GetWonPileCount().ToString();
        _player2WarCountLabel.text = string.Format(_warLabelFormatText, player2.GetPlayerWarsWon());
    }
    /// <summary>
    /// Callback for Slider UI control
    /// </summary>
    public void TimeSliderMoved() {
        Time.timeScale = Mathf.Lerp(_timeScaleMin, _timeScaleMax, _timeScaleSlider.value);
    }
}
