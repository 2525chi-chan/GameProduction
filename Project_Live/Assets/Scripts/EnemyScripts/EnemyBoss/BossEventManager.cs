using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class BossEventManager : MonoBehaviour//ボス登場イベントの管理
{
    [SerializeField] FadeManager fadeManager;//フェード
    [SerializeField]BossSpawnManager bossSpawnManager;//ボス生成マネージャー
    [SerializeField] PlayerInput playerInput;
    [SerializeField]float fadeWaitTime = 1f;

    GameObject player;
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public IEnumerator BossEvent()
    {
        //ボス登場イベント
        playerInput.SwitchCurrentActionMap("Movie");
        yield return StartCoroutine(fadeManager.FadeIn());
        player.transform.position = new(0, 2, 0);//プレイヤーの位置リセット
        yield return StartCoroutine(fadeManager.FadeOut());
        bossSpawnManager.SpawnBoss();


        yield return new WaitForSeconds(fadeWaitTime);

        //ゲーム切り替え
        yield return StartCoroutine(fadeManager.FadeIn());
 yield return new WaitForSeconds(fadeWaitTime);
        yield return StartCoroutine(fadeManager.FadeOut());
       


        playerInput.SwitchCurrentActionMap("Player");
    }
}
