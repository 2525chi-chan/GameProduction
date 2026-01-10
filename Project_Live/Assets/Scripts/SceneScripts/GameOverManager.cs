using UnityEngine;
using System.Collections;
public class GameOverManager : MonoBehaviour//ƒQ[ƒ€ƒI[ƒo[‰æ–Ê‘JˆÚ‚ðŠÇ—‚·‚é
{
    [Header("Ž€–SŒãƒQ[ƒ€ƒI[ƒo[‰æ–Ê‚É‘JˆÚ‚·‚é‚Ü‚Å‚ÌŽžŠÔ")]
    [SerializeField]  float gameOverTime = 2f;
    [SerializeField] BuzuriRank rank;
    [SerializeField] GoodSystem good;
    public static GameOverManager Instance { get; private set; }

    void Awake() => Instance = this;
    public void StartGameOver()
    {
        StartCoroutine(GameOverInterval());
    }
    public  IEnumerator GameOverInterval()
    {
       
        yield return new WaitForSeconds(gameOverTime);
        BazuriShotHolder.Instance.GetRank(rank.CurrentIndex);
        BazuriShotHolder.Instance.GetGood((int)good.GoodNum);
        Debug.Log("GameOver!!!!");
        SelectScene.LoadScene(SelectScene.SceneName.GameOver);

    }
}
