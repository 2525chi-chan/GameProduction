using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnParameter
{
    [Header("��������G�v���n�u")]
    public GameObject enemyPrefab;
    [Header("���̓G�̍ő哯���o����")]
    public int maxSpawnCount;
    [Header("���̓G�̈ړ��^�C�v")]
    public EnemyMover.EnemyMoveType moveType;
}

public class EnemySpawnManager : BaseSpawnManager
{
    [Header("��������G�̐ݒ�")]
    [SerializeField] List<SpawnParameter> spawnParameters;
    [Header("���̓G��|������Đ������I��点�邩")]
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
                Debug.Log("�G�����I��");
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

    public void SetUpEnemySpawns() //�G�����̏����ݒ�
    {
        foreach (var param in spawnParameters)
        {
            var key = (param.enemyPrefab, param.moveType);

            spawners[key] = new EnemySpawn(param.enemyPrefab, spawnArea, param.moveType);
            trackers[key] = new EnemyCountTracker(param.enemyPrefab, param.moveType);

            spawners[key].SpawnEnemies(param.maxSpawnCount, param.moveType);
            trackers[key].ForceSync();
            //Debug.Log($"{key} �� {EnemyRegistry.GetCount(param.enemyPrefab, param.moveType)} �̐������܂���");
        }
    }

    public void RespawnProcess() //�G�̍Đ�������
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
                //Debug.Log($"{key} �� {toSpawn} �̍Đ������܂���");
            }
        }
    }
}