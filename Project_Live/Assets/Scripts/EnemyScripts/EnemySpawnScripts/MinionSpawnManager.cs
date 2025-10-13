using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnMinionParameter
{
    [Header("生成する敵プレハブ")]
    public GameObject enemyPrefab;
    [Header("この敵の最大同時出現数")]
    public int maxSpawnCount;
}
public class MinionSpawnManager : BaseSpawnManager
{
    [Header("生成する敵の設定")]
    [SerializeField] List<SpawnMinionParameter> minionParameters;

    EnemyMover mover;

    void Start()
    {
        SetUpEnemySpawns();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpEnemySpawns() //敵生成の初期設定
    {
        mover = GetComponentInParent<EnemyMover>(); //このクラスをアタッチされたオブジェクトを子として持つ親の移動コンポーネントを取得する

        if (mover == null) return;

        foreach (var param in minionParameters)
        {
            spawners[param.enemyPrefab] = new EnemySpawn(param.enemyPrefab, spawnArea, mover.MoveType);
            trackers[param.enemyPrefab] = new EnemyCountTracker(param.enemyPrefab);
            spawners[param.enemyPrefab].SpawnEnemies(param.maxSpawnCount);
        }
    }

    public void RespawnProcess() //敵の再生成処理
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;

        foreach (var param in minionParameters)
        {
            var tracker = trackers[param.enemyPrefab];

            if (timer >= checkInterval && tracker.HasChanged(out int currentCount)) //調べた種類の敵の数が少なくなっていたら
            {
                timer = 0f;

                int toSpawn = param.maxSpawnCount - currentCount; //その種類の敵の最大同時出現数と現在の数との差分を求める

                if (toSpawn > 0) spawners[param.enemyPrefab].SpawnEnemies(toSpawn); //少ない分だけ敵を生成する
            }
        }
    }
}
