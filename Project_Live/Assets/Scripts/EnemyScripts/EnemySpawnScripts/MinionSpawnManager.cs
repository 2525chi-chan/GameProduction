using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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

    public void SetUpEnemySpawns() //�G�����̏����ݒ�
    {
        mover = GetComponentInParent<EnemyMover>(); //���̃N���X���A�^�b�`���ꂽ�I�u�W�F�N�g���q�Ƃ��Ď��e�̈ړ��R���|�[�l���g���擾����

        if (mover == null) return;

        foreach (var param in minionParameters)
        {
            spawners[param.enemyPrefab] = new EnemySpawn(param.enemyPrefab, spawnArea, mover.MoveType);
            trackers[param.enemyPrefab] = new EnemyCountTracker(param.enemyPrefab);
            spawners[param.enemyPrefab].SpawnEnemies(param.maxSpawnCount);
        }
    }

    public void RespawnProcess() //�G�̍Đ�������
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;

        foreach (var param in minionParameters)
        {
            var tracker = trackers[param.enemyPrefab];

            if (timer >= checkInterval && tracker.HasChanged(out int currentCount)) //���ׂ���ނ̓G�̐������Ȃ��Ȃ��Ă�����
            {
                timer = 0f;

                int toSpawn = param.maxSpawnCount - currentCount; //���̎�ނ̓G�̍ő哯���o�����ƌ��݂̐��Ƃ̍��������߂�

                if (toSpawn > 0) spawners[param.enemyPrefab].SpawnEnemies(toSpawn); //���Ȃ��������G�𐶐�����
            }
        }
    }
}
