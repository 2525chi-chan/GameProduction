using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectScene: MonoBehaviour
{
    private static SceneName nextScene;
   public enum SceneName
{
    Menu,
    Loading,
    Main,
    GameClear,
    GameOver
}

    public static void LoadScene(SceneName target)
    {
        nextScene = target;
        SceneManager.LoadScene("LoadingScene");
    }
    public static SceneName ReturnNext()
    {
        return nextScene;
    }
   

}
