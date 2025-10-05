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
    public EnemyMover.MoveType moveType;
}

public class EnemySpawnManager : MonoBehaviour
{
    [Header("�X�|�[��������I�u�W�F�N�g�̐ݒ�")]
    [SerializeField] List<SpawnParameter> spawnParameters;
    
    [Header("�����͈�")]
    [SerializeField] BoxCollider spawnArea;
    [Header("�G���Đ������邩�ǂ���")]
    [SerializeField] bool enableRespawn = true;
    [Header("�G�̐��̃`�F�b�N�Ԋu")]
    [SerializeField] float checkInterval = 1.0f;

    Dictionary<GameObject, EnemySpawn> spawners = new();
    Dictionary<GameObject, EnemyCountTracker> trackers = new();

    float timer = 0f;

    void Start()
    {
        foreach (var param in spawnParameters) //�ݒ肳�ꂽ�G�̎�ނ̐������������J��Ԃ�
        {
            spawners[param.enemyPrefab] = new EnemySpawn(param.enemyPrefab, spawnArea, param.moveType);
            trackers[param.enemyPrefab] = new EnemyCountTracker(param.enemyPrefab);
            spawners[param.enemyPrefab].SpawnEnemies(param.maxSpawnCount); // ��������
        }
    }

    void Update()
    {
        if (!enableRespawn) return;

        timer += Time.deltaTime;

        foreach (var param in spawnParameters)
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