using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnManager : MonoBehaviour
{
    [Header("生成範囲")]
    [SerializeField] protected BoxCollider spawnArea;
    [Header("再生成を行うか")]
    [SerializeField] protected bool enableRespawn = true;
    [Header("敵数のチェック間隔")]
    [SerializeField] protected float checkInterval = 1.0f;

    protected Dictionary<GameObject, EnemySpawn> spawners = new();
    protected Dictionary<GameObject, EnemyCountTracker> trackers = new();

    protected float timer = 0f;
}
