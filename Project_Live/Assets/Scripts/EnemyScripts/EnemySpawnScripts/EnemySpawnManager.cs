using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnRankSettings
{
    [Header("カーブ補間を使用するか")]
    public bool useCurve = true;
    [Tooltip("UseCurveのチェックを外した場合、敵の生成数がこの数で固定されます。")]
    [Header("カーブ補間を使用しない場合の固定生成数")]
    public int fixedSpawnCount = 1;
    [Tooltip("この秒数でカーブが0→1まで進みます。\n設定した秒数が経過した後、カーブの始点に戻り再度遷移を始めます。")]
    [Header("このランクのカーブ適用時間（秒）")]
    public float curveDuration = 10f;
    [Tooltip("敵の生成数の遷移を管理するグラフです。\n" +
        "\n各キー（グラフ上の点）を右クリック → Edit Key... を選択して、時間とその時点の敵の生成数（value）を設定できます。timeの値は0～1の範囲内に収めようにしてください。\n" +
        "\n編集できるグラフが表示されていない場合は、下に表示されているグラフの図から適用したいグラフの形状をクリックしてください。")]
    [Header("生成数カーブ（x = 0～1）")]
    public AnimationCurve spawnCurve = AnimationCurve.Linear(0f, 1f, 1f, 10f);
}

[System.Serializable]
public class SpawnParameter
{
    [Header("生成する敵プレハブ")]
    public GameObject enemyPrefab;
    [Header("バズリランクごとの敵の生成数の設定")]
    [Tooltip("Element 0～4の要素が、プチバズ～神バズの順番に対応しています。")]
    public List<SpawnRankSettings> countSetting_bazuriRank;
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

    float curveTimer = 0f; //同ランク間における敵数増減補間用の変数
    int previousRankIndex = -1;

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

        ResetCurveTimerOnRankChange();
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
                int maxSpawn = GetMaxSpawnCount(param, curveTimer);

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
        curveTimer += Time.deltaTime;
        
        if (timer < checkInterval) return;
        timer = 0f;

        foreach (var param in spawnParameters)
        {
            var key = (param.enemyPrefab, param.moveType);

            int maxSpawn = GetMaxSpawnCount(param, curveTimer);
            ResetCurveTimer(param, ref curveTimer);
            int currentCount = EnemyRegistry.GetCount(param.enemyPrefab, param.moveType);

            int toSpawn = maxSpawn - currentCount;

            if (toSpawn > 0)
            {
                spawners[key].SpawnEnemies(toSpawn, param.moveType);
                //Debug.Log($"{key} を {toSpawn} 体再生成しました");
            }
        }
    }

    int GetMaxSpawnCount(SpawnParameter param, float timer) //現在のランクに合わせた敵の生成数を取り出す
    {
        if (bazuriRank == null) return param.countSetting_bazuriRank[0].fixedSpawnCount;

        int rankIndex = bazuriRank.CurrentIndex;

        if (rankIndex < 0 || rankIndex >= param.countSetting_bazuriRank.Count)
            return 0;

        var rankSettings = param.countSetting_bazuriRank[rankIndex];

        if (!rankSettings.useCurve) return rankSettings.fixedSpawnCount;

        float normalizedTime = timer / rankSettings.curveDuration;
        normalizedTime = Mathf.Clamp01(normalizedTime);

        int spawnCount = (int)param.countSetting_bazuriRank[rankIndex].spawnCurve.Evaluate(normalizedTime);

        return spawnCount;
    }

    void ResetCurveTimer(SpawnParameter param, ref float timer) //カーブに使用するタイマーのリセット
    {
        if (bazuriRank == null) return;

        int rankIndex = bazuriRank.CurrentIndex;
        if (rankIndex < 0 || rankIndex >= param.countSetting_bazuriRank.Count)
            return;

        if (curveTimer >= param.countSetting_bazuriRank[rankIndex].curveDuration)
            curveTimer = 0f;
    }

    void ResetCurveTimerOnRankChange() //ランク変更時のタイマーのリセット
    {
        if (bazuriRank == null) return;
        
        int currentRank = bazuriRank.CurrentIndex;

        if (currentRank == previousRankIndex) return;

        curveTimer = 0f;
        previousRankIndex = currentRank;
    }
}