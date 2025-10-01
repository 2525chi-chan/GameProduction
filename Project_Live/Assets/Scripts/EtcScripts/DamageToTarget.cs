using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�쐬�ҁF�K��

public class DamageToTarget : MonoBehaviour
{
    [Header("�������ɔ�������G�t�F�N�g")]
    [SerializeField] GameObject damageEffect;

    GameObject hitEffect;

    float damage;
    float forwardKnockbackForce;
    float upwardKnockbackForce;

    public GameObject HitEffect { get { return hitEffect; } set { hitEffect = value; } }
    
    public float Damage { get { return damage; } set { damage = value; } }
    public float ForwardKnockbackForce { get { return forwardKnockbackForce; } set { forwardKnockbackForce = value; } }
    public float UpwardKnockbackForce { get { return upwardKnockbackForce; } set { upwardKnockbackForce = value; } }

    public void AddDamageToPlayer(Collider player) //�_���[�W��^����
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();

        // �Ȃ���Ύq�I�u�W�F�N�g����T��
        if (playerStatus == null)  playerStatus = player.GetComponentInChildren<PlayerStatus>();

        if (playerStatus == null)
        {
            Debug.LogWarning($"PlayerStatus �� {player.name} ����сA���̎q�Ɍ�����܂���ł���");
            return;
        }
        //

        //�v���C���[����𒆂̓_���[�W��^���Ȃ��悤�ɂ���
        if (playerStatus.CurrentState == PlayerStatus.PlayerState.Invincible)
        {
            //Debug.Log("�v���C���[�͍U�����������");
            return;
        }

        playerStatus.Hp -= damage;
        Debug.Log(damage + "�_���[�W��^����");

        if (hitEffect != null) Instantiate(hitEffect, player.bounds.center, player.gameObject.transform.rotation); //�G�t�F�N�g���ݒ肳��Ă�����A�������ɃG�t�F�N�g�𐶐�����
    }

    public void AddDamageToEnemy(Collider enemy)
    {
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();

        // �Ȃ���Ύq���I�u�W�F�N�g����T��
        if (enemyStatus == null) enemyStatus = enemy.GetComponentInChildren<EnemyStatus>();

        if (enemyStatus == null)
        {
            Debug.LogWarning($"EnemyStatus �� {enemy.name} ����сA���̎q�Ɍ�����܂���ł���");
            return;
        }
        //

        enemyStatus.Hp -= damage;
        Debug.Log(damage + "�_���[�W��^����");

        if (hitEffect != null) Instantiate(hitEffect, enemy.bounds.center, enemy.gameObject.transform.rotation); //�G�t�F�N�g���ݒ肳��Ă�����A�������ɃG�t�F�N�g�𐶐�����
    }

    public void AddDamageToObject(Collider obj) //�_���[�W��^����
    {
        ObjectStatus objStatus = obj.GetComponent<ObjectStatus>();

        // �Ȃ���Ύq�I�u�W�F�N�g����T��
        if (objStatus == null) objStatus = obj.GetComponentInChildren<ObjectStatus>();

        if (objStatus == null)
        {
            Debug.LogWarning($"PlayerStatus �� {obj.name} ����сA���̎q�Ɍ�����܂���ł���");
            return;
        }
        //

        objStatus.Hp -= damage;
        Debug.Log(damage + "�_���[�W��^����");

        if (hitEffect != null) Instantiate(hitEffect, obj.bounds.center, obj.gameObject.transform.rotation); //�G�t�F�N�g���ݒ肳��Ă�����A�������ɃG�t�F�N�g�𐶐�����
    }

    public void ApplyKnockback(Collider target) //������ԗ͂�������
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Vector3 forwardDirection = (target.transform.position - transform.position).normalized;

            Vector3 knockback = forwardDirection * forwardKnockbackForce + Vector3.up * upwardKnockbackForce;

            rb.AddForce(knockback, ForceMode.Impulse);
        }
    }
}
