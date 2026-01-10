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
        var async = UnityEngine.SceneManagement.SceneManager
            .LoadSceneAsync(GetNextSceneName());
        async.allowSceneActivation = false;

        float displayed = 0f;

        while (async.progress < 0.9f)
        {
            float raw = async.progress / 0.9f;     // 0〜1
            float eased = Mathf.SmoothStep(0f, 1f, raw); // 緩やかに加速

            // 表示は eased に追いつくように補間
            displayed = Mathf.MoveTowards(displayed, eased, Time.deltaTime * 0.5f);
            slider.value = displayed;
            yield return null;
        }

        // 0.9 以降は 1.0 に近づける
        while (displayed < 0.99f)
        {
            displayed = Mathf.MoveTowards(displayed, 1f, Time.deltaTime * 0.5f);
            slider.value = displayed;
            yield return null;
        }

        // 好きなタイミングで遷移
        async.allowSceneActivation = true;
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
            case SelectScene.SceneName.Guide:
                return "GuideScene";
            case SelectScene.SceneName.GameClear:
            return "GameClearScene";

            case SelectScene.SceneName.GameOver:
            return "GameOverScene";  

            default:return "MainScene";
        }
      
       
    }
}
