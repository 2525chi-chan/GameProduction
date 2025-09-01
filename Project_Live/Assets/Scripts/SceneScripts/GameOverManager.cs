using UnityEngine;
using System.Collections;
public class GameOverManager : MonoBehaviour//ƒQ[ƒ€ƒI[ƒo[‰æ–Ê‘JˆÚ‚ðŠÇ—‚·‚é
{
    [Header("Ž€–SŒãƒQ[ƒ€ƒI[ƒo[‰æ–Ê‚É‘JˆÚ‚·‚é‚Ü‚Å‚ÌŽžŠÔ")]
    [SerializeField]  float gameOverTime = 2f;

    public static GameOverManager Instance { get; private set; }

    void Awake() => Instance = this;
    public void StartGameOver()
    {
        StartCoroutine(GameOverInterval());
    }
    public  IEnumerator GameOverInterval()
    {
       
        yield return new WaitForSeconds(gameOverTime);
 Debug.Log("GameOver!!!!");
        SelectScene.LoadScene(SelectScene.SceneName.GameOver);

    }
}
