using UnityEngine;

public class MainMenu : MonoBehaviour{
    public string GameSceneName;

    public void PlayButton(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameSceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void QuitGameButton(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
