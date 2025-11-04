using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class EnemyCountManager : MonoBehaviour//敵の数を管理するクラス
{
    [SerializeField] BossEventManager bossManager;
    [SerializeField] int bossSpawnCount;//ボスを呼ぶためのカウント
    List<GameObject> enemies = new List<GameObject>();

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
        if (!bossSpawned && deadCount > bossSpawnCount)
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
