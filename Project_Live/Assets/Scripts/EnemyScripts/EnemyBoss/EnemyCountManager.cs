using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class EnemyCountManager : MonoBehaviour//“G‚Ì”‚ğŠÇ—‚·‚éƒNƒ‰ƒX
{
    [SerializeField] BossEventManager bossManager;
    List<GameObject> enemies = new List<GameObject>();
    public static EnemyCountManager instance;

    bool bossSpawned = false;
    private void Start()
    {
        instance = this;
    }

    public void Update()
    {
        if(enemies.Count == 0&&!bossSpawned)
        {
            Debug.Log("All enemies defeated!");
           
            bossSpawned = true;
            StartCoroutine(bossManager.BossEvent());
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    public void UnregisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
