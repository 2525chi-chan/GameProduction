using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType //“G‚ÌŽí—Þ
{
    Normal, Eliet, Shooter
}

public class EnemyIdentifier : MonoBehaviour
{
    GameObject prefabReference;

    public void Initialize(GameObject prefab)
    {
        prefabReference = prefab;
        EnemyRegistry.Register(prefabReference);
    }

    void OnEnable()
    {
        if (prefabReference != null)
            EnemyRegistry.Register(prefabReference);
    }

    void OnDestroy()
    {
        if (prefabReference != null)
            EnemyRegistry.Unregister(prefabReference);
    }
}
