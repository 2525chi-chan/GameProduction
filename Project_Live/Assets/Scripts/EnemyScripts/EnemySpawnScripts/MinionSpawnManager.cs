using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private List<SpawnMinionParameter> minionParameters;

    private EnemyMover mover;

    void Start()
    {
        SetUpEnemySpawns();
    }

    void Update()
    {
        if (!enableRespawn) return;
        RespawnProcess();
    }

    public void SetUpEnemySpawns() //生成の初期設定
    {
        mover = GetComponentInParent<EnemyMover>(); //親のEnemyMoverを取得
        if (mover == null)
        {
            //Debug.Log("MinionSpawnManager: 親に EnemyMover が見つかりません。");
            return;
        }

        foreach (var param in minionParameters)
        {
            var key = (param.enemyPrefab, mover.MoveType);

            spawners[key] = new EnemySpawn(param.enemyPrefab, spawnArea, mover.MoveType);
            trackers[key] = new EnemyCountTracker(param.enemyPrefab, mover.MoveType);

            spawners[key].SpawnEnemies(param.maxSpawnCount, mover.MoveType);
            trackers[key].ForceSync();
            //Debug.Log($"{key} を {EnemyRegistry.GetCount(param.enemyPrefab, mover.MoveType)} 体生成しました");
        }
    }

    public void RespawnProcess() //再生成処理
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        foreach (var param in minionParameters)
        {
            var key = (param.enemyPrefab, mover.MoveType);
            int currentCount = EnemyRegistry.GetCount(param.enemyPrefab, mover.MoveType);
            int toSpawn = param.maxSpawnCount - currentCount;

            if (toSpawn > 0)
            {
                spawners[key].SpawnEnemies(toSpawn, mover.MoveType);
                //Debug.Log($"{key} を {toSpawn} 体再生成しました");
            }
        }
    }
}
