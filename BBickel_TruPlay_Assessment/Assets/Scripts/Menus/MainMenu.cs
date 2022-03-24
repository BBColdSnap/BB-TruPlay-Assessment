using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu - Basic UI interface for landing or starting a game
/// </summary>
public class MainMenu : MonoBehaviour{

    //Inspector Fields
    [SerializeField]
    private GameObject _titlePanel;                     //Root object for Title UI
    [SerializeField]
    private GameObject _optionsPanel;                   //Root object for Options UI
    [SerializeField]
    private string _gameSceneName;                      //Scene to load for sameplay
    [SerializeField]
    private Toggle _shortenedGamesToggle;               //Toggle reference to turn on/off checkmark for Shortened Games
    [SerializeField]
    private Toggle _cardChoiceToggle;                   //Toggle reference to turn on/off checkmark for Player Card Choices

    private void Start() {
        _titlePanel.SetActive(true);
        _optionsPanel.SetActive(false);

        _shortenedGamesToggle.isOn = PlayerPrefs.GetInt(GameLogic.ShortenedGamesKey) == 1;
        _cardChoiceToggle.isOn = PlayerPrefs.GetInt(GameLogic.PlayerChoiceKey) == 1;
    }
    /// <summary>
    /// PlayButton callback connected to prefab. Loads the _gameSceneName Scene
    /// </summary>
    public void PlayButton(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(_gameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void OptionsButton() {
        _titlePanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }
    /// <summary>
    /// QuitGameButton callback connected to prefab. Closes the application.
    /// </summary>
    public void QuitGameButton(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void ShortGamesButton() {
        _shortenedGamesToggle.isOn = !_shortenedGamesToggle.isOn;
        PlayerPrefs.SetInt(GameLogic.ShortenedGamesKey, _shortenedGamesToggle.isOn ? 1 : 0);
    }
    public void CardChoicebutton() {
        _cardChoiceToggle.isOn = !_cardChoiceToggle.isOn;
        PlayerPrefs.SetInt(GameLogic.PlayerChoiceKey, _cardChoiceToggle.isOn ? 1 : 0);
    }
    public void OptionsBackButton() {
        _titlePanel.SetActive(true);
        _optionsPanel.SetActive(false);
    }
}
