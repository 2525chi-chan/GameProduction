using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnMinionParameter
{
    [Header("��������G�v���n�u")]
    public GameObject enemyPrefab;
    [Header("���̓G�̍ő哯���o����")]
    public int maxSpawnCount;
}

public class MinionSpawnManager : BaseSpawnManager
{
    [Header("��������G�̐ݒ�")]
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

    public void SetUpEnemySpawns() //�����̏����ݒ�
    {
        mover = GetComponentInParent<EnemyMover>(); //�e��EnemyMover���擾
        if (mover == null)
        {
            //Debug.Log("MinionSpawnManager: �e�� EnemyMover ��������܂���B");
            return;
        }

        foreach (var param in minionParameters)
        {
            var key = (param.enemyPrefab, mover.MoveType);

            spawners[key] = new EnemySpawn(param.enemyPrefab, spawnArea, mover.MoveType);
            trackers[key] = new EnemyCountTracker(param.enemyPrefab, mover.MoveType);

            spawners[key].SpawnEnemies(param.maxSpawnCount, mover.MoveType);
            trackers[key].ForceSync();
            //Debug.Log($"{key} �� {EnemyRegistry.GetCount(param.enemyPrefab, mover.MoveType)} �̐������܂���");
        }
    }

    public void RespawnProcess() //�Đ�������
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
                //Debug.Log($"{key} �� {toSpawn} �̍Đ������܂���");
            }
        }
    }
}
