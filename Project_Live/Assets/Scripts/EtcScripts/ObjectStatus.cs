using UnityEngine;

public class ObjectStatus : CharacterStatus
{
    [Header("破壊時に発生するエフェクト")]
    [SerializeField] GameObject destroyEffect;


    [Header("回復速度")]
    public float recoveryRate = 1f;
    [Header("回復までの時間")]
    public float recoveryTime;
    bool isBroken = false;

    public bool IsBroken { get { return isBroken; } }

    private float count;
    public float Count
    {
        get { return count; }

        set { count = value; }
    }
    float prevHp;
    // Update is called once per frame
    private void Start()
    {
        prevHp= Hp;
    }
    void Update()
    {
        count += Time.deltaTime;
        if (count >= recoveryTime && Hp < MaxHp && !isBroken)
        {
            Hp += recoveryRate * Time.deltaTime;
            if (Hp > MaxHp)
            {
                Hp = MaxHp;
            }
        }
        if (prevHp < Hp)
        {
            ResetTime();
        }
            prevHp= Hp;

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
    public void ResetTime()
    {
        count = 0f;
    }
}
