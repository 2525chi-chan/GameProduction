using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class EnemyCountManager : MonoBehaviour//敵の数を管理するクラス
{
    [SerializeField] BossEventManager bossManager;
    [SerializeField] int bossSpawnCount;//ボスを呼ぶためのカウント
    [SerializeField] bool isbossSpawn = false;//ボスが湧くかどうか
    List<GameObject> enemies = new ();

    public static EnemyCountManager instance;

    bool bossSpawned = false;
    int deadCount;
    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        //if(enemies.Count == 0&&!bossSpawned)
        //{
        //    Debug.Log("All enemies defeated!");

        //    bossSpawned = true;
        //    StartCoroutine(bossManager.BossEvent());
        //}
        if (!bossSpawned && deadCount > bossSpawnCount&&isbossSpawn)
        { Debug.Log("aa!!!");
            bossSpawned = true;
            StartCoroutine(bossManager.BossEvent());
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    public void UnregisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            deadCount++;
           
        }
    }
}
