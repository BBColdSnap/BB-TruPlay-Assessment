using UnityEngine;

/// <summary>
/// MainMenu - Basic UI interface for landing or starting a game
/// </summary>
public class MainMenu : MonoBehaviour{

    //Inspector Fields
    [SerializeField]
    private string _gameSceneName;                      //Scene to load for sameplay

    /// <summary>
    /// PlayButton callback connected to prefab
    /// </summary>
    public void PlayButton(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(_gameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    /// <summary>
    /// QuitGameButton callback connected to prefab
    /// </summary>
    public void QuitGameButton(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
