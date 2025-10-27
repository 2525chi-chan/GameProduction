using UnityEngine;

public class EnemyIdentifier : MonoBehaviour
{
    private GameObject prefabReference;
    private EnemyMover.EnemyMoveType moveType;
    private bool isRegistered = false;

    public void Awake()
    {
        EnemyCountManager.instance.RegisterEnemy(this.gameObject);
    }
    public void Initialize(GameObject prefab, EnemyMover.EnemyMoveType moveType)
    {
        prefabReference = prefab;
        this.moveType = moveType;

        Register();
    }

    private void Register()
    {
        if (!isRegistered && prefabReference != null)
        {
            EnemyRegistry.Register(gameObject, prefabReference, moveType);
            isRegistered = true;
        }
    }

    private void Unregister()
    {
        if (isRegistered && prefabReference != null)
        {
            EnemyRegistry.Unregister(gameObject, prefabReference, moveType);
            isRegistered = false;
            EnemyCountManager.instance.UnregisterEnemy(this.gameObject);
        }
    }

    void OnEnable()
    {
        if (prefabReference != null && !isRegistered) Register();
    }

    void OnDisable()
    {
        Unregister();
    }

    void OnDestroy()
    {
        Unregister();
    }
}
