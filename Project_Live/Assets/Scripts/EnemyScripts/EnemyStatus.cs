using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyStatus : CharacterStatus
{
    [Header("HPが0になってから消えるまでの時間")]
    [SerializeField] float destroyDuration = 1f;
    public float DestroyDuration
    {
        get { return destroyDuration; }
        set { destroyDuration = value; }
    }
    [Header("撃破時のエフェクト")]
    [SerializeField] GameObject deathEffect;


    [Header("後ろに吹き飛ばされる力")]
    [SerializeField] float knockbackForce_Back = 5f;
    public float KnockBackForce_Back
    {
        get { return knockbackForce_Back; }
        set { knockbackForce_Back = value; }
    }
    [Header("上に吹き飛ばされる力")]
    [SerializeField] float knockbackForce_Up = 0.5f;
 public float KnockBackForce_Up
    {
        get { return knockbackForce_Up; }
        set { knockbackForce_Up = value; }
    }

   

    

   
    bool isDead = false;
    Rigidbody rb;
    [Header("いいね獲得数")]
    [SerializeField] float getGoodNum;

    GoodSystem goodSystem;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        

        goodSystem = GameObject.FindWithTag("GoodSystem").GetComponent<GoodSystem>();

        
    }

    private void Update()
    {
        if (!isDead && Hp <= 0) Die();
    }

    void Die()
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        // 向いている方向の反対に吹っ飛ぶ
        Vector3 backwardForce = -transform.forward * knockbackForce_Back;
        Vector3 upwardForce = Vector3.up * knockbackForce_Up;

        // 吹き飛ばす
        rb.AddForce(backwardForce + upwardForce, ForceMode.Impulse);

        Destroy(gameObject, destroyDuration);

        //いいね取得
        goodSystem.AddGood(getGoodNum);
        //
    }
}
