using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType //�G�̎��
{
    Normal, Eliet, Shooter
}

public class EnemyTypeIdentifier : MonoBehaviour
{
    public EnemyType type;

    void OnEnable()
    {
        EnemyRegistry.Register(type);
    }

    void OnDestroy()
    {
        EnemyRegistry.Unregister(type);
    }
}
