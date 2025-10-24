using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnParameter
{
    [Header("¶¬‚·‚é“GƒvƒŒƒnƒu")]
    public GameObject enemyPrefab;
    [Header("‚±‚Ì“G‚ÌÅ‘å“¯oŒ»”")]
    public int maxSpawnCount;
    [Header("‚±‚Ì“G‚ÌˆÚ“®ƒ^ƒCƒv")]
    public EnemyMover.EnemyMoveType moveType;
}

public class EnemySpawnManager : BaseSpawnManager
{
    [Header("¶¬‚·‚é“G‚Ìİ’è")]
    [SerializeField] List<SpawnParameter> spawnParameters;
    [Header("‰½‘Ì“G‚ğ“|‚µ‚½‚çÄ¶¬‚ğI‚í‚ç‚¹‚é‚©")]
    [SerializeField] int  spawnEndCount;

    private int defeatedEnemyCount = 0;
    public int DefeatedEnemyCount
    {
        get { return defeatedEnemyCount; }
        set
        {
            defeatedEnemyCount = value;
            if (defeatedEnemyCount >= spawnEndCount)
            {
                enableRespawn = false;
                Debug.Log("“G¶¬I—¹");
            }
        }
    }
    void Start()
    {
        SetUpEnemySpawns();
    }

    void Update()
    {
        if (!enableRespawn) return;
        RespawnProcess();
    }

    public void SetUpEnemySpawns() //“G¶¬‚Ì‰Šúİ’è
    {
        foreach (var param in spawnParameters)
        {
            var key = (param.enemyPrefab, param.moveType);

            spawners[key] = new EnemySpawn(param.enemyPrefab, spawnArea, param.moveType);
            trackers[key] = new EnemyCountTracker(param.enemyPrefab, param.moveType);

            spawners[key].SpawnEnemies(param.maxSpawnCount, param.moveType);
            trackers[key].ForceSync();
            //Debug.Log($"{key} ‚ğ {EnemyRegistry.GetCount(param.enemyPrefab, param.moveType)} ‘Ì¶¬‚µ‚Ü‚µ‚½");
        }
    }

    public void RespawnProcess() //“G‚ÌÄ¶¬ˆ—
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        foreach (var param in spawnParameters)
        {
            var key = (param.enemyPrefab, param.moveType);
            int currentCount = EnemyRegistry.GetCount(param.enemyPrefab, param.moveType);
            int toSpawn = param.maxSpawnCount - currentCount;

            if (toSpawn > 0)
            {
                spawners[key].SpawnEnemies(toSpawn, param.moveType);
                //Debug.Log($"{key} ‚ğ {toSpawn} ‘ÌÄ¶¬‚µ‚Ü‚µ‚½");
            }
        }
    }
}