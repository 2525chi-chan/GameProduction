using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class EnemyStatus : CharacterStatus
{
    [Header("HP��0�ɂȂ��Ă�����ł���܂ł̎���")]
    [SerializeField] float destroyDuration = 1f;
    public float DestroyDuration
    {
        get { return destroyDuration; }
        set { destroyDuration = value; }
    }
    [Header("���S���̃G�t�F�N�g")]
    [SerializeField] GameObject deathEffect;


    [Header("������ԗ́i���j")]
    [SerializeField] float knockbackForce_Back = 5f;
    public float KnockBackForce_Back
    {
        get { return knockbackForce_Back; }
        set { knockbackForce_Back = value; }
    }
    [Header("������ԗ́i��j")]
    [SerializeField] float knockbackForce_Up = 0.5f;

    //�������M

    [Header("�l���ł��邢���ː�")]
    [SerializeField] float getGoodNum;

    GoodSystem goodSystem;

    //

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

        //�������M

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

        // ������я����i�O���� + �������ʂɉ��Z�j
        Vector3 backwardForce = -transform.forward * knockbackForce_Back;
        Vector3 upwardForce = Vector3.up * knockbackForce_Up;

        // �������ēK�p
        rb.AddForce(backwardForce + upwardForce, ForceMode.Impulse);

        Destroy(gameObject, destroyDuration);

        //�������M
        goodSystem.AddGood(getGoodNum);
        //
    }
}
