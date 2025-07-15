using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    [Header("死亡時に発生するエフェクト")]
    [SerializeField] GameObject deathEffect;
    [Header("消滅させるオブジェクト")]
    [SerializeField] GameObject target;

    bool isDead = false;
    Rigidbody rb;

    public bool IsDead { get { return isDead; } set { isDead = value; } }

    public enum PlayerState
    {
        Normal, Invincible
    }

    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDead && Hp <= 0) Die();
    }

    void Die() //死亡時の処理
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(target);
    }
}
