using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class LoadingGame : MonoBehaviour
{

    [SerializeField]Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadGame());
    }
    public IEnumerator LoadGame()
    {

        AsyncOperation async=UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GetNextSceneName());
        while (!async.isDone)
        {
            var progress = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }

    }
    
    public string GetNextSceneName()
    {
        switch(SelectScene.ReturnNext())
        {
            case SelectScene.SceneName.Menu:
            return "MenuScene";
                
            case SelectScene.SceneName.Loading:
            return "LoadingScene";

            case SelectScene.SceneName.Main:
            return "MainScene";

            case SelectScene.SceneName.GameClear:
            return "GameClearScene";

            case SelectScene.SceneName.GameOver:
            return "GameOverScene";  

            default:return "MainScene";
        }
      
       
    }
}
