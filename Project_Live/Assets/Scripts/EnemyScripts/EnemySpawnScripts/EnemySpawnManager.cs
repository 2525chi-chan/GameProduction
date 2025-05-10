using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("��������I�u�W�F�N�g")]
    [SerializeField] GameObject enemyPrefab;
    [Header("�I�u�W�F�N�g�̍ő�o�����i���̐������A�G����ɃX�e�[�W��ɂ���")]
    [SerializeField] int maxSpawnCount = 10;
    [Header("�����͈�")]
    [SerializeField] BoxCollider spawnArea;
    [Header("�G�̐��̃`�F�b�N�Ԋu")]
    [SerializeField] float checkInterval = 1.0f;

    EnemySpawn spawner;
    EnemyCountTracker tracker;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawner = new EnemySpawn(enemyPrefab, spawnArea);
        tracker = new EnemyCountTracker("Enemy");

        spawner.SpawnEnemies(maxSpawnCount);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < checkInterval) return;

        if (tracker.HasChanged(out int currentCount)) //�G�̐��ɕω�����������
        {
            int toSpawn = maxSpawnCount - currentCount; //�G�̐ݒ葍���ƁA���݂̐��̍��������߂�

            if (toSpawn > 0) spawner.SpawnEnemies(toSpawn); //����Ȃ��������G�𐶐�
        }
    }
}