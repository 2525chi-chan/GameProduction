using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class EnemyStatus : CharacterStatus
{
    [Header("HPが0になってから消滅するまでの時間")]
    [SerializeField] float destroyDuration = 1f;

    [Header("死亡時のエフェクト")]
    [SerializeField] GameObject deathEffect;

    [Header("吹き飛ぶ力（奥）")]
    [SerializeField] float knockbackForce_Back = 5f;

    [Header("吹き飛ぶ力（上）")]
    [SerializeField] float knockbackForce_Up = 0.5f;

    //寺村加筆

    [Header("獲得できるいいね数")]
    [SerializeField] float getGoodNum;

    GoodSystem goodSystem;

    //

    bool isDead = false;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //寺村加筆

        goodSystem = GameObject.FindWithTag("GoodSystem").GetComponent<GoodSystem>();

        //
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

        //寺村加筆
        goodSystem.AddGood(getGoodNum);
        //
    }
}
