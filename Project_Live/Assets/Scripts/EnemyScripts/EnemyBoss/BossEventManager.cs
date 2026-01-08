using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class BossEventManager : MonoBehaviour//ボス登場イベントの管理
{
    [SerializeField] FadeManager fadeManager;//フェード
    [SerializeField]BossSpawnManager bossSpawnManager;//ボス生成マネージャー
    [SerializeField] PlayerInput playerInput;
    [SerializeField]PlayerStatus playerStatus;
    [SerializeField]float fadeWaitTime = 1f;
    [SerializeField] float playerInvincibilityTime = 1f;

    GameObject player;
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    public IEnumerator BossEvent()
    {
      //  playerStatus.CurrentState = PlayerStatus.PlayerState.Invincible;
        //ボス登場イベント
        playerInput.SwitchCurrentActionMap("Movie");
        playerStatus.ToggleInvincible(); //プレイヤーを無敵状態に切り替える
        yield return StartCoroutine(fadeManager.FadeIn());
        player.transform.position = new(0, 2, 0);//プレイヤーの位置リセット

        yield return new WaitForSeconds(fadeWaitTime);
        yield return StartCoroutine(fadeManager.FadeOut());
        bossSpawnManager.SpawnBoss();


        yield return new WaitForSeconds(fadeWaitTime);

        //ゲーム切り替え
        yield return StartCoroutine(fadeManager.FadeIn());
        yield return new WaitForSeconds(fadeWaitTime);
        yield return StartCoroutine(fadeManager.FadeOut());

     //   playerStatus.CurrentState = PlayerStatus.PlayerState.Normal;

        playerInput.SwitchCurrentActionMap("Player");

        yield return new WaitForSeconds(playerInvincibilityTime);
        playerStatus.ToggleInvincible(); //プレイヤーの無敵状態を解除する
    }
}
