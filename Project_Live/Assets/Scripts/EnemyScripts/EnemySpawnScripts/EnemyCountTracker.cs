using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting;
using UnityEngine;

public class EnemyCountTracker
{
    GameObject enemyPrefab;
    int prev_Count;

    public EnemyCountTracker(GameObject enemyPrefab)
    {
        this.enemyPrefab = enemyPrefab;
        prev_Count = -1;
    }

    public bool HasChanged(out int currentCount) //“G‚Ì”‚ª•Ï‚í‚Á‚½‚Ç‚¤‚©”»’è‚·‚é
    {
        currentCount = EnemyRegistry.GetCount(enemyPrefab);

        if (currentCount != prev_Count)
        {
            prev_Count = currentCount;
            return true;
        }

        return false;
    }
}
