using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    public enum SceneName
    {
        Menu,
        Loading,
        Main,
        GameClear,
        GameOver
    }

    // シングルトン
    public static SelectScene Instance { get; private set; }

    [SerializeField] private FadeManager fadeManager;

    private static SceneName nextScene;

    void Awake()
    {
        // 適宜シングルトン化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // 必要なら
    }

    // ボタンなどから呼ぶ static API
    public static void LoadScene(SceneName target)
    {
        // インスタンスに委譲
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.LoadCoroutine(target));
        }
        else
        {
            // フェードなしフォールバック
            nextScene = target;
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public static SceneName ReturnNext()
    {
        return nextScene;
    }

    // 実処理はインスタンス側
    private IEnumerator LoadCoroutine(SceneName target)
    {
        if (fadeManager != null)
        {
            yield return fadeManager.FadeIn();
        }

        nextScene = target;
        SceneManager.LoadScene("LoadingScene");
    }
}
