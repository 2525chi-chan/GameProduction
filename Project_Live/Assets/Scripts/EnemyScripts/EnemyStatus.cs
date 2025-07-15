using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class EnemyStatus : CharacterStatus
{
    [Header("HPが0になってから消滅するまでの時間")]
    [SerializeField] float destroyDuration = 1f;
    public float DestroyDuration
    {
        get { return destroyDuration; }
        set { destroyDuration = value; }
    }
    [Header("死亡時のエフェクト")]
    [SerializeField] GameObject deathEffect;


    [Header("吹き飛ぶ力（奥）")]
    [SerializeField] float knockbackForce_Back = 5f;
    public float KnockBackForce_Back
    {
        get { return knockbackForce_Back; }
        set { knockbackForce_Back = value; }
    }
    [Header("吹き飛ぶ力（上）")]
    [SerializeField] float knockbackForce_Up = 0.5f;
    public float KnockBackForce_Up
    {
        get { return knockbackForce_Up; }
        set { knockbackForce_Up = value; }
    }
    bool isDead = false;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isDead && Hp <= 0) Die();
    }

    void Die()
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        // 吹き飛び処理（前方向 + 上方向を個別に加算）
        Vector3 backwardForce = -transform.forward * knockbackForce_Back;
        Vector3 upwardForce = Vector3.up * knockbackForce_Up;

        // 合成して適用
        rb.AddForce(backwardForce + upwardForce, ForceMode.Impulse);

        Destroy(gameObject, destroyDuration);
    }
}
