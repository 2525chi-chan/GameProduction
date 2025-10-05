using UnityEngine;

public class ObjectStatus : CharacterStatus
{
    [Header("破壊時に発生するエフェクト")]
    [SerializeField] GameObject destroyEffect;

    bool isBroken = false;

    public bool IsBroken { get { return isBroken; } }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0 && !isBroken)
        {
            DestroyProcess();
        }
    }

    void DestroyProcess()
    {
        if (!isBroken)
        {
            isBroken = true; //破壊判定
            if (destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity); //エフェクトの生成
            Destroy(gameObject); //この建物を破壊する
        }
    }
}
