using System.Collections;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    [SerializeField] float clearInterval;
    [SerializeField] BuzuriRank rank;
    [SerializeField] GoodSystem good;
    public void StartClearSequence()
    {
       StartCoroutine(GameClearInterval());
    }

    public IEnumerator GameClearInterval()
    {
        BazuriShotHolder.Instance.GetRank(rank.CurrentIndex);
        BazuriShotHolder.Instance.GetGood((int)good.GoodNum);

               yield return new WaitForSeconds(clearInterval);
        Debug.Log("GameClear!!!!");
        SelectScene.LoadScene(SelectScene.SceneName.GameClear);
    }
}
