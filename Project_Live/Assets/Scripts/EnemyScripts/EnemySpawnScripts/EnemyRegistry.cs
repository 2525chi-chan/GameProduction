using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyRegistry
{
    static Dictionary<GameObject, int> enemyCounts = new();

    public static void Register(GameObject prefab) //指定された種類の敵のカウントを増やす
    {
        if (!enemyCounts.ContainsKey(prefab))
            enemyCounts[prefab] = 0;

        enemyCounts[prefab]++;
    }

    public static void Unregister(GameObject prefab) //指定された種類の敵のカウントを減らす
    {
        if (!enemyCounts.ContainsKey(prefab)) return;

        enemyCounts[prefab]--;

        if (enemyCounts[prefab] <= 0) enemyCounts[prefab] = 0;
    }

    public static int GetCount(GameObject prefab) //指定された種類の敵が現在存在している数を取得する
    {
        return enemyCounts.TryGetValue(prefab, out var count) ? count : 0;
    }
}