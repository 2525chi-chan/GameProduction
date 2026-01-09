using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Scripting;
using UnityEngine;

public class EnemyCountTracker
{
    GameObject prefab;
    EnemyMover.EnemyMoveType moveType;

    public EnemyCountTracker(GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        this.prefab = prefab;
        this.moveType = moveType;
    }

    public int ForceSync()
    {
        return EnemyRegistry.GetCount(prefab, moveType);
    }
}
