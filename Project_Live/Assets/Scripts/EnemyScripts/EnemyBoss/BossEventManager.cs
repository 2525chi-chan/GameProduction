using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class BossEventManager : MonoBehaviour//ボス登場イベントの管理
{
    [SerializeField] Transform resetPos;
    [SerializeField]BoxCollider boxCollider_Player;
    [SerializeField] BoxCollider boxCollider_Remove;
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

        EnemyMoveSet(true);

        yield return StartCoroutine(fadeManager.FadeIn());
        player.transform.position = resetPos.position;//プレイヤーの位置リセット

        yield return new WaitForSeconds(fadeWaitTime);
        yield return StartCoroutine(fadeManager.FadeOut());
        bossSpawnManager.SpawnBoss();

        RemoveEnemyTransform();
        yield return new WaitForSeconds(fadeWaitTime);

        //ゲーム切り替え
        yield return StartCoroutine(fadeManager.FadeIn());
        yield return new WaitForSeconds(fadeWaitTime);
        yield return StartCoroutine(fadeManager.FadeOut());
        RemoveEnemyTransform();
        //   playerStatus.CurrentState = PlayerStatus.PlayerState.Normal;

        playerInput.SwitchCurrentActionMap("Player");

        yield return new WaitForSeconds(playerInvincibilityTime);
        playerStatus.ToggleInvincible(); //プレイヤーの無敵状態を解除する

        EnemyMoveSet(false);
    }


    public void EnemyMoveSet(bool isStop)
    {

        EnemyStatus[] enemies = FindObjectsByType<EnemyStatus>(sortMode:FindObjectsSortMode.None);

        foreach (EnemyStatus enemy in enemies)
        {
            Debug.Log(enemy);
            enemy.ISBossSpawn = isStop;
        }
        RemoveEnemyTransform();


    }
    public void RemoveEnemyTransform()
    {
        // ワールド座標での中心
        Vector3 center = boxCollider_Player.transform.TransformPoint(boxCollider_Player.center);
        // halfExtents は bounds.extents を使う
        Vector3 halfExtents = boxCollider_Player.bounds.extents;

        Collider[] nearEnemies =
            Physics.OverlapBox(center, halfExtents, boxCollider_Player.transform.rotation);

        foreach (var enemy in nearEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.transform.position = GetRandomPositionInsideCollider();
                Debug.Log(enemy.name);
            }
        }
    }

    public Vector3 GetRandomPositionInsideCollider() //生成位置の取得
    {
        Vector3 center = boxCollider_Remove.transform.TransformPoint(boxCollider_Remove.center);
        //  Vector3 center = boxCollider_Remove.center + boxCollider_Remove.transform.position;
        Vector3 size = Vector3.Scale(boxCollider_Remove.size, boxCollider_Remove.transform.lossyScale);

        float x = Random.Range(-size.x / 2, size.x / 2);
        float z = Random.Range(-size.z / 2, size.z / 2);
        float y = 0f;

        return center + new Vector3(x, y, z);
    }
}
