using System.Collections;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    [SerializeField] float clearInterval;
   
    public void StartClearSequence()
    {
       StartCoroutine(GameClearInterval());
    }

    public IEnumerator GameClearInterval()
    {
               yield return new WaitForSeconds(clearInterval);
        Debug.Log("GameClear!!!!");
        SelectScene.LoadScene(SelectScene.SceneName.GameClear);
    }
}
