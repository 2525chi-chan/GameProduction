using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyRegistry
{
    static Dictionary<GameObject, int> enemyCounts = new();

    public static void Register(GameObject prefab) //�w�肳�ꂽ��ނ̓G�̃J�E���g�𑝂₷
    {
        if (!enemyCounts.ContainsKey(prefab))
            enemyCounts[prefab] = 0;

        enemyCounts[prefab]++;
    }

    public static void Unregister(GameObject prefab) //�w�肳�ꂽ��ނ̓G�̃J�E���g�����炷
    {
        if (!enemyCounts.ContainsKey(prefab)) return;

        enemyCounts[prefab]--;

        if (enemyCounts[prefab] <= 0) enemyCounts[prefab] = 0;
    }

    public static int GetCount(GameObject prefab) //�w�肳�ꂽ��ނ̓G�����ݑ��݂��Ă��鐔���擾����
    {
        return enemyCounts.TryGetValue(prefab, out var count) ? count : 0;
    }
}