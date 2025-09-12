using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnParameter
{
    [Header("生成する敵プレハブ")]
    public GameObject enemyPrefab;
    [Header("この敵の最大同時出現数")]
    public int maxSpawnCount;
    [Header("この敵の種類")]
    public EnemyType enemyType;
}

public class EnemySpawnManager : MonoBehaviour
{
    [Header("スポーンさせるオブジェクトの設定")]
    [SerializeField] List<SpawnParameter> spawnParameters;
    [Header("生成範囲")]
    [SerializeField] BoxCollider spawnArea;
    [Header("敵を再生成するかどうか")]
    [SerializeField] bool enableRespawn = true;
    [Header("敵の数のチェック間隔")]
    [SerializeField] float checkInterval = 1.0f;

    Dictionary<EnemyType, EnemySpawn> spawners = new();
    Dictionary<EnemyType, EnemyCountTracker> trackers = new();

    float timer = 0f;

    void Start()
    {
        foreach (var param in spawnParameters) //設定された敵の種類の数だけ処理を繰り返す
        {
            spawners[param.enemyType] = new EnemySpawn(param.enemyPrefab, spawnArea);
            trackers[param.enemyType] = new EnemyCountTracker(param.enemyType);
            spawners[param.enemyType].SpawnEnemies(param.maxSpawnCount); //敵の初期生成
        }
    }

    void Update()
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;

        foreach (var param in spawnParameters)
        {
            var tracker = trackers[param.enemyType];

            if (timer >= checkInterval && tracker.HasChanged(out int currentCount)) //調べた種類の敵の数が少なくなっていたら
            {
                timer = 0f;

                int toSpawn = param.maxSpawnCount - currentCount; //その種類の敵の最大同時出現数と現在の数との差分を求める

                if (toSpawn >= 0) spawners[param.enemyType].SpawnEnemies(toSpawn); //少ない分だけ敵を生成する
            }
        }
    }
}