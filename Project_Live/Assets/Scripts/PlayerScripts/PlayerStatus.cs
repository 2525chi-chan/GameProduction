using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    [Header("���S���ɔ�������G�t�F�N�g")]
    [SerializeField] GameObject deathEffect;
    [Header("���ł�����I�u�W�F�N�g")]
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

    void Die() //���S���̏���
    {
        isDead = true;

        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(target);
    }
}
