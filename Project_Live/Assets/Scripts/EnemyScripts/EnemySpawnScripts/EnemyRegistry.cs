using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyRegistry
{
    static Dictionary<(GameObject prefab, EnemyMover.EnemyMoveType moveType), List<GameObject>> registry = new();

    //�G�̓o�^
    public static void Register(GameObject enemy, GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        var key = (prefab, moveType);
        if (!registry.ContainsKey(key))
            registry[key] = new List<GameObject>();
        registry[key].Add(enemy);
    }

    //�G�̓o�^����
    public static void Unregister(GameObject enemy, GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        var key = (prefab, moveType);
        if (registry.ContainsKey(key))
            registry[key].Remove(enemy);
    }

    //�G�̐��̎擾
    public static int GetCount(GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        var key = (prefab, moveType);
        return registry.ContainsKey(key) ? registry[key].Count : 0;
    }
}