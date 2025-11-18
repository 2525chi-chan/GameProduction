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

    [Header("ランクごとの最大同時出現数")]
    [Tooltip("0～4個目の要素が、プチバズ～神バズの順番に対応しているものとして設定してください。")]
    public List<int> maxSpawnCount_bazuriRank;

    [Header("ランクごとの敵数のゆらぎ ±値")]
    [Tooltip("同じランクでも出現数を変化させたい場合に使用してください。")]
    public List<int> fluctuation_bazuriRank;

    [Header("この敵の移動タイプ")]
    public EnemyMover.EnemyMoveType moveType;
}

public class EnemySpawnManager : BaseSpawnManager
{
    [Header("生成する敵の設定")]
    [SerializeField] List<SpawnParameter> spawnParameters;
    //[Header("何体敵を倒したら再生成を終わらせるか")]
    //[SerializeField] int  spawnEndCount;
    [Header("ゲーム開始時に敵を生成するか")]
    [SerializeField] bool spawnOnStart = true;
    [Header("必要なコンポーネント")]
    [SerializeField] BuzuriRank bazuriRank;

    private int defeatedEnemyCount = 0;
    public int DefeatedEnemyCount
    {
        get { return defeatedEnemyCount; }
        set
        {
            defeatedEnemyCount = value;
            //if (defeatedEnemyCount >= spawnEndCount)
            //{
            //    enableRespawn = false;
            //    Debug.Log("敵生成終了");
            //}
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

    public void SetUpEnemySpawns() //敵生成の初期設定
    {
        foreach (var param in spawnParameters)
        {
            var key = (param.enemyPrefab, param.moveType);

            spawners[key] = new EnemySpawn(param.enemyPrefab, spawnArea, param.moveType);
            trackers[key] = new EnemyCountTracker(param.enemyPrefab, param.moveType);

            if (spawnOnStart)
            {
                int maxSpawn = GetMaxSpawnCount(param);

                //spawners[key].SpawnEnemies(param.maxSpawnCount, param.moveType);
                spawners[key].SpawnEnemies(maxSpawn, param.moveType);
                trackers[key].ForceSync();
            }

            
            //Debug.Log($"{key} を {EnemyRegistry.GetCount(param.enemyPrefab, param.moveType)} 体生成しました");
        }
    }

    public void RespawnProcess() //敵の再生成処理
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        foreach (var param in spawnParameters)
        {
            var key = (param.enemyPrefab, param.moveType);

            int maxSpawn = GetMaxSpawnCount(param);
            int currentCount = EnemyRegistry.GetCount(param.enemyPrefab, param.moveType);

            //int toSpawn = param.maxSpawnCount - currentCount;
            int toSpawn = maxSpawn - currentCount;

            if (toSpawn > 0)
            {
                spawners[key].SpawnEnemies(toSpawn, param.moveType);
                //Debug.Log($"{key} を {toSpawn} 体再生成しました");
            }
        }
    }

    int GetMaxSpawnCount(SpawnParameter param) //現在のランクに合わせた敵の生成数を取り出す
    {
        int rankIndex = bazuriRank.CurrentIndex;

        if (rankIndex < 0 || rankIndex >= param.maxSpawnCount_bazuriRank.Count)
            return 0;

        int baseCount = param.maxSpawnCount_bazuriRank[rankIndex];

        //ゆらぎが設定されていなければ固定値
        if (param.fluctuation_bazuriRank == null
            || rankIndex >= param.fluctuation_bazuriRank.Count)
            return baseCount;

        int fluct = param.fluctuation_bazuriRank[rankIndex];

        int min = Mathf.Max(0, baseCount - fluct);
        int max = baseCount + fluct;

        //return param.maxSpawnCount_bazuriRank[rankIndex];
        return Random.Range(min, max + 1);
    }
}