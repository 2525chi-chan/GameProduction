using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyRegistry
{
   static Dictionary<(GameObject prefab, EnemyMover.EnemyMoveType moveType), List<GameObject>> registry = new();

    public static Dictionary<(GameObject prefab, EnemyMover.EnemyMoveType moveType), List<GameObject>> Registry
    {
        get { return registry; }

    }

    //“G‚Ì“o˜^
    public static void Register(GameObject enemy, GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        var key = (prefab, moveType);
        if (!registry.ContainsKey(key))
            registry[key] = new List<GameObject>();
        registry[key].Add(enemy);
    }

    //“G‚Ì“o˜^‰ğœ
    public static void Unregister(GameObject enemy, GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        var key = (prefab, moveType);
        if (registry.ContainsKey(key))
            registry[key].Remove(enemy);
    }

    //“G‚Ì”‚Ìæ“¾
    public static int GetCount(GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        var key = (prefab, moveType);
        return registry.ContainsKey(key) ? registry[key].Count : 0;
    }
}