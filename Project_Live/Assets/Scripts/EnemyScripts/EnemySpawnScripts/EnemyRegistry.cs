using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyRegistry
{
    static Dictionary<EnemyType, int> enemyCounts = new();

    public static void Register(EnemyType type) //�w�肳�ꂽ��ނ̓G�̃J�E���g�𑝂₷
    {
        if (!enemyCounts.ContainsKey(type))
            enemyCounts[type] = 0;

        enemyCounts[type]++;
    }

    public static void Unregister(EnemyType type) //�w�肳�ꂽ��ނ̓G�̃J�E���g�����炷
    {
        if (!enemyCounts.ContainsKey(type)) return;

        enemyCounts[type]--;

        if (enemyCounts[type] <= 0) enemyCounts[type] = 0;
     }

    public static int GetCount(EnemyType type) //�w�肳�ꂽ��ނ̓G�����ݑ��݂��Ă��鐔���擾����
    {
        return enemyCounts.TryGetValue(type, out var count) ? count : 0;
    }
}