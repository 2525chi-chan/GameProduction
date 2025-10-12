using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnManager : MonoBehaviour
{
    [Header("�����͈�")]
    [SerializeField] protected BoxCollider spawnArea;
    [Header("�Đ������s����")]
    [SerializeField] protected bool enableRespawn = true;
    [Header("�G���̃`�F�b�N�Ԋu")]
    [SerializeField] protected float checkInterval = 1.0f;

    protected Dictionary<GameObject, EnemySpawn> spawners = new();
    protected Dictionary<GameObject, EnemyCountTracker> trackers = new();

    protected float timer = 0f;
}
